using System.Runtime.CompilerServices;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic;
using TestSystem.Database.Cosmos.Repositories;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Azure.Cosmos;
using System;
using System.Diagnostics;

namespace TestSystem.ExtensionMethods
{
	internal static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddProjectServices(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddBusinessLogicServices()
				.AddDatabaseServices(configuration);

			return services;
		}

		private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
		{
			services.AddScoped<IDatabaseInitializationManager, DatabaseInitializationManager>();
			services.AddScoped<ITestsManager, TestsManager>();
			services.AddScoped<IUsersManager, UsersManager>();

			return services;
		}

		private static IServiceCollection AddDatabaseServices(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddScoped<IDatabaseStructureRepository, DatabaseStructureRepository>();
			services.AddScoped<ITestsRepository, TestsRepository>();
			services.AddScoped<IUsersRepository, UsersRepository>();
			
			services.AddScoped<CosmosClient>((serviceProvider) =>
			{
				var uri = configuration["uri"];
				var key = configuration["key"];
				return new CosmosClient(uri, key);
			});


			return services;
		}

	}
}
