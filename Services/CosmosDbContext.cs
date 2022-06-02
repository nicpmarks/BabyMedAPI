using Microsoft.EntityFrameworkCore;
using BabyMedsAPI.Models;

namespace BabyMedsAPI.Services
{
	public class CosmosDbContext : DbContext
	{
		public CosmosDbContext(DbContextOptions<CosmosDbContext> options) : base(options)
        {
			Database.EnsureCreated();
        }

		public DbSet<Medicine> Medicines { get; set; }

	}
}