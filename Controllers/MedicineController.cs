using Microsoft.AspNetCore.Mvc;
using BabyMedsAPI.Models;
using BabyMedsAPI.Services;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace BabyMedsAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicineController : ControllerBase
	{
		[HttpGet]
   		public async Task<string> GetAll() 
		{
			string str = "";
  			foreach (Medicine med in _context.Medicines) 
			{
				string jsonString = JsonSerializer.Serialize(med);
				str += jsonString;
			}

			return str;
		}



		private readonly CosmosDbContext _context;

    	public MedicineController(CosmosDbContext cosmosDbContext) =>
        	_context = cosmosDbContext;
			
		/*
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
		*/
	}

}