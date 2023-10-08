using System.Runtime.CompilerServices;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic;
using TestSystem.Database.Cosmos.Repositories;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Azure.Cosmos;
using System;
using System.Diagnostics;
using TestSystem.Database.SqlServer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TestSystem.Database.SqlServer;

namespace TestSystem.ExtensionMethods
{
	internal static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddProjectServices(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddBusinessLogicServices()
				.AddSQLServerServices(configuration);//comment this line and use next line for cosmos db
				//.AddCosmosDBServices(configuration);

			return services;
		}

		private static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
		{
			services.AddScoped<IDatabaseInitializationManager, DatabaseInitializationManager>();
			services.AddScoped<ITestsManager, TestsManager>();
			services.AddScoped<IUsersManager, UsersManager>();

			return services;
		}

		private static IServiceCollection AddCosmosDBServices(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddScoped<IDatabaseStructureRepository, Database.Cosmos.Repositories.DatabaseStructureRepository>();
			services.AddScoped<ITestsRepository, Database.Cosmos.Repositories.TestsRepository>();
			services.AddScoped<IUsersRepository, Database.Cosmos.Repositories.UsersRepository>();
			
			services.AddScoped<CosmosClient>((serviceProvider) =>
			{
				var uri = configuration["uri"];
				var key = configuration["key"];
				return new CosmosClient(uri, key);
			});


			return services;
		}

		private static IServiceCollection AddSQLServerServices(this IServiceCollection services, ConfigurationManager configuration)
		{
			services.AddScoped<IDatabaseStructureRepository, Database.SqlServer.Repositories.DatabaseStructureRepository>();
			services.AddScoped<ITestsRepository, Database.SqlServer.Repositories.TestsRepository>();
			services.AddScoped<IUsersRepository, Database.SqlServer.Repositories.UsersRepository>();
			services.AddDbContext<TestSystemContext>((provider, options) =>
			{
				var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
				options.UseSqlServer(configuration.GetConnectionString("TestSystemContext"))
					.UseLoggerFactory(loggerFactory);
			});

			return services;
		}


	}
}
