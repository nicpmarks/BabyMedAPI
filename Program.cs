using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;

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
			
			var dbSettings = new CosmosDBSettings();
			builder.Configuration.GetSection("CosmosDB").Bind(dbSettings);

			builder.Services.AddDbContext<CosmosDbContext>(options => options.UseCosmos(
				dbSettings.APIURI,
				dbSettings.APIKEY,
				dbSettings.DBName
			));

			builder.Services.AddControllers();


			var app = builder.Build();

			
			

			app.UseRouting();

			
			app.MapGet("/", () => "Hello");
			app.MapControllers();

			app.Run();

        }
		
    }

}



