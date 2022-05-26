using Microsoft.AspNetCore.Mvc;
using BabyMedsAPI.Models;
using BabyMedsAPI.Services;
using System.Text.Json;

namespace BabyMedsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicineController : ControllerBase
	{
		private readonly CosmosDBService _cosmosDbService;

    	public MedicineController(CosmosDBService cosmosDbService) =>
        	_cosmosDbService = cosmosDbService;

		[HttpGet]
   		public async Task<string> GetAll() 
		{
        	var medList = await _cosmosDbService.GetAllMedicinesAsync();
			return JsonSerializer.Serialize(medList);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Medicine>> Get(string id)
		{
			var med = await _cosmosDbService.GetMedicineAsync(id);

			if (med is null)
			{
				return NotFound();
			}

			return med;
		}

		[HttpPost]
		public async Task<IActionResult> Post(Medicine med)
		{
			await _cosmosDbService.AddMedicineAsync(med);

			return CreatedAtAction(nameof(Get), new { id = med.Id }, med);
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> Update(string id, Medicine updatedMed)
		{
			var med = await _cosmosDbService.GetMedicineAsync(id);

			if (med is null)
			{
				return NotFound();
			}

			updatedMed.Id = med.Id;

			await _cosmosDbService.UpdateMedicineAsync(id, updatedMed);

			return NoContent();
		}

		[HttpPost("delete/{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var med = await _cosmosDbService.GetMedicineAsync(id);

			if (med is null)
			{
				return NotFound();
			}

			await _cosmosDbService.RemoveMedicineAsync(id);

			return NoContent();
		}
	}

}