using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LinguaDecks.DataAccess.Context
{
	public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
	{
		public ApplicationContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
				.AddJsonFile($"appsettings.{environment}.json")
				.Build();
			string connectionString = configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
			return new ApplicationContext(optionsBuilder.Options);
		}
	}
}
