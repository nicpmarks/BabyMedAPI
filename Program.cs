using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;

using BabyMedsAPI.Services;
using BabyMedsAPI.Controllers;
using BabyMedsAPI.Models;


namespace BabyMedsAPI 
{
	class Program
    {

        static void Main(string[] args)
        {
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.Configure<CosmosDBSettings>(builder.Configuration.GetSection("CosmosDB"));
			builder.Services.AddSingleton<CosmosDBService>();
			builder.Services.AddControllers();


			var app = builder.Build();

			
			

			app.UseRouting();

			
			app.MapGet("/", () => "Hello");
			app.MapControllers();

			app.Run();

        }
		
    }

}



