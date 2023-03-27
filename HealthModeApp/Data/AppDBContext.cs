using System;
using Microsoft.EntityFrameworkCore;
using HealthModeApp.Models.SQLite;

namespace HealthModeApp.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) :base(options)
		{

		}

		public DbSet<FoodModel> FoodData => Set<FoodModel>();
	}
}

