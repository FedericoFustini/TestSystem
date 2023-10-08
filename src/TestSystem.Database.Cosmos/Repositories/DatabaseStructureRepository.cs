using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Database.Cosmos.Models.testsContainer;

namespace TestSystem.Database.Cosmos.Repositories
{
	internal class DatabaseStructureRepository : IDatabaseStructureRepository
	{
		private readonly CosmosClient _cosmosClient;
		private readonly ILogger<DatabaseStructureRepository> _logger;

		public DatabaseStructureRepository(CosmosClient cosmosClient, ILogger<DatabaseStructureRepository> logger)
		{
			_cosmosClient = cosmosClient;
			_logger = logger;
		}

		public async Task CreateDatabase()
		{
			var throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(4000);
			Microsoft.Azure.Cosmos.Database cosmosDatabase = await _cosmosClient.CreateDatabaseIfNotExistsAsync(DBConstants.TEST_DATABASE, throughputProperties);

			var testsContainer = CreateTestContainerProperties();

			ContainerProperties usersContainer = CreateUserContainerProperties();
			var tasks = new List<Task>();
			tasks.Add(cosmosDatabase.CreateContainerIfNotExistsAsync(testsContainer, throughputProperties));
			tasks.Add(cosmosDatabase.CreateContainerIfNotExistsAsync(usersContainer, throughputProperties));
			await Task.WhenAll(tasks);
		}

		private ContainerProperties CreateUserContainerProperties()
		{
			var containerProp = new ContainerProperties()
			{
				Id = DBConstants.USER_CONTAINER,
				PartitionKeyPaths = new List<string>()
				{
					"/UserId",
					"/TestId"
				},
			};

			var keys = new UniqueKey();
			keys.Paths.Add("/UserId");
			keys.Paths.Add("/TestId");
			keys.Paths.Add("/QuestionId");
			containerProp.UniqueKeyPolicy.UniqueKeys.Add(keys);
			return containerProp;
		}

		private ContainerProperties CreateTestContainerProperties()
		{
			var testsContainer = new ContainerProperties()
			{
				Id = DBConstants.TEST_CONTAINER,
				PartitionKeyPaths = new List<string>()
				{
					"/TestId",
					"/QuestionId"
				},
			};

			return testsContainer;
		}

		public async Task<bool> IsEmpty()
		{
			var countTest = await CountTestContainerItem();
			var countUsers = await CountUsersContainerItem();

			return countTest == 0 && countUsers == 0;
		}

		private async Task<int> CountTestContainerItem()
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var qry = container.GetItemQueryIterator<int>("SELECT VALUE COUNT(c.testId) FROM c");
			int? count = null;
			while (qry.HasMoreResults)
			{
				count = (await qry.ReadNextAsync()).Single();
			}

			return count.Value;
		}

		private async Task<int> CountUsersContainerItem()
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);
			var qry = container.GetItemQueryIterator<int>("SELECT VALUE COUNT(c.userId) FROM c");
			int? count = null;
			while (qry.HasMoreResults)
			{
				count = (await qry.ReadNextAsync()).Single();
			}

			return count.Value;
		}

		public async Task PopulateDatabase(IEnumerable<TestToGenerate> tests)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);

			var tasks = new List<Task>();
			var testId = 1;
			foreach (var test in tests)
			{
				var questionId = 1;
				tasks.Add(CreateTestItem(container, test, testId));
				foreach (var question in test.PossibleQuestions)
				{
					tasks.Add(CreateQuestionItem(container, question, testId++, questionId++));
					_logger.LogInformation("Wrote test {testId} and question {questionId}", testId, questionId);
				}
			}

			await Task.WhenAll(tasks);
		}

		private async Task CreateQuestionItem(Container container, QuestionToGenerate question, int testId, int questionId)
		{
			var questionItem = new QuestionDB(question, testId, questionId);
			// The full partition key path is not necessary but i specified for underlying the use of hierarchical partition key
			var partitionKey = new PartitionKeyBuilder()
						.Add(questionItem.TestId)
						.Add(questionItem.QuestionId)
						.Build();

			_ = await container.CreateItemAsync(questionItem, partitionKey);
		}

		private async Task CreateTestItem(Container container, TestToGenerate test, int testId)
		{
			var testItem = new TestDB(test, testId);
			// The full partition key path is not necessary but i specified for underlying the use of hierarchical partition key
			var partitionKey = new PartitionKeyBuilder()
						.Add(testItem.TestId)
						.Add(testItem.QuestionId)
						.Build();

			_ = await container.CreateItemAsync(testItem, partitionKey);
		}

		public async Task DeleteDatabase()
		{
			var cosmosDatabase = _cosmosClient.GetDatabase(DBConstants.TEST_DATABASE);
			await cosmosDatabase.DeleteAsync();
		}
	}
}
