using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using BabyMedsAPI.Models;
using System.Linq;

namespace BabyMedsAPI.Services
{
	public class CosmosDBService
    {

		private string apiUri;
		private string apiKey;
		private string dbName;
		private string medicineContainerId = "MedicineContainer";
		private string historyContainerId = "HistoryContainer";
		private string [] containerNames;
		private CosmosClient cosmosClient;
		public bool dbCreated = false;
		public CosmosDBService(IOptions<CosmosDBSettings> mongoDBSettings) 
		{
			apiUri = mongoDBSettings.Value.APIURI;
			apiKey = mongoDBSettings.Value.APIKEY;
			dbName = mongoDBSettings.Value.DBName;
			containerNames = new string [] {medicineContainerId, historyContainerId};
			cosmosClient = new CosmosClient(apiUri, apiKey);

		}

		public async Task AddMedicineAsync(Medicine medicine) 
		{

			var container = cosmosClient.GetContainer(dbName, medicineContainerId);
			try
			{
				// Read the item to see if it exists.  
				ItemResponse<Medicine> medicineResponse = await container.ReadItemAsync<Medicine>(medicine.Id, new PartitionKey(medicine.Id));
				Console.WriteLine("Item in database with name: {0} already exists\n", medicineResponse.Resource.Name);
			}
			catch(CosmosException ex)
			{
				ItemResponse<Medicine> medicineResponse = await container.CreateItemAsync<Medicine>(medicine, new PartitionKey(medicine.Id));
				Console.WriteLine("Created item in database with name: {0}\n", medicineResponse.Resource.Name);
			}	
		
		} 

		public async Task<List<Medicine>> GetAllMedicinesAsync()
		{
			var medList = new List<Medicine>();

			var container = cosmosClient.GetContainer(dbName, medicineContainerId);
			foreach (Medicine medicine in container.GetItemLinqQueryable<Medicine>(true))
			{
				medList.Add(medicine);
				Console.WriteLine("Got in database with name: {0}\n", medicine.Name);
			}


			return medList;
		}

		public async Task<Medicine> GetMedicineAsync(string id)
		{


			var container = cosmosClient.GetContainer(dbName, medicineContainerId);
			try
			{
				ItemResponse<Medicine> medicineResponse = await container.ReadItemAsync<Medicine>(id, new PartitionKey(id));
				Console.WriteLine("Got in database with name: {0}\n", medicineResponse.Resource.Name);
				return medicineResponse.Resource;				
			}
			catch (CosmosException ex) 
			{		
				Console.WriteLine("Med ID {0} not found",id);		
			}
			return null;

		}

		public async Task UpdateMedicineAsync(string id, Medicine med)  
        {  
			var container = cosmosClient.GetContainer(dbName, medicineContainerId);
            await container.UpsertItemAsync<Medicine>(med, new PartitionKey(id));  
			Console.WriteLine("Updated name: {0}\n", med.Name);
        }  

		

		public async Task RemoveMedicineAsync(string id)  
        { 

			var container = cosmosClient.GetContainer(dbName, medicineContainerId);
			await container.DeleteItemAsync<Medicine>(id, new PartitionKey(id));  
			Console.WriteLine("Deleted id: {0}\n", id);

        }  

		public async Task RunTestAsync() 
		{
			Medicine testMed = new Medicine() {Id="0", Name="Tylenol"};
			await AddMedicineAsync(testMed);
			await AddMedicineAsync(testMed);
		}

		public async Task DeleteServiceAsync()
		{
		
			var db = cosmosClient.GetDatabase(dbName);
			DatabaseResponse databaseResourceResponse = await db.DeleteAsync();

			Console.WriteLine("Deleted Database: {0}\n", dbName);

		}

		private async Task GenerateDBIfNotExistsAsync() 
		{
			
			var dbResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
			Console.WriteLine("Created Database: {0}\n", dbResponse.Database.Id);

			foreach (string containerName in containerNames) 
			{
				ContainerProperties containerProperties = new ContainerProperties(containerName, "/id");
				var containerResponse = await dbResponse.Database.CreateContainerIfNotExistsAsync(containerProperties);
				Console.WriteLine("Created Container: {0}\n", containerResponse.Container.Id);
			}		


			dbCreated = true;
		}	

	}

}