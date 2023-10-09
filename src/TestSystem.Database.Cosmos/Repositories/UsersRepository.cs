using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Database.Cosmos.Models.testsContainer;
using TestSystem.Database.Cosmos.Models.usersContainer;

namespace TestSystem.Database.Cosmos.Repositories
{
	internal class UsersRepository : IUsersRepository
	{
		private readonly CosmosClient _cosmosClient;

		public UsersRepository(CosmosClient cosmosClient)
		{
			_cosmosClient = cosmosClient;
		}

		public async Task<Guid> CreateUser(string username, int testId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);
			var userId = Guid.NewGuid();
			var userItem = new UserTestResultDB(userId, username, testId);
			// The full partition key path is not necessary but i specified for underlying the use of hierarchical partition key
			var partitionKey = new PartitionKeyBuilder()
						.Add(userItem.UserId.ToString())
						.Add(userItem.TestId)
						.Build();

			_ = await container.CreateItemAsync(userItem, partitionKey);
			return userId;
		}

		public async Task<bool> Exist(Guid userId, int testId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);
			var query = @$"SELECT  VALUE COUNT(u.UserId)
FROM users u
WHERE u.UserId = '{userId}'
AND u.TestId = {testId}
AND u.Type = 'testResult'";
			using var results = container.GetItemQueryIterator<int>(query);

			var count = 0;
			while (results.HasMoreResults)
				count = (await results.ReadNextAsync()).Single();

			return count != 0;
		}

		public async Task<bool> ExistAnswer(Guid userId, int testId, int questionId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);
			var query = @$"SELECT  VALUE COUNT(u.UserId)
FROM users u
WHERE u.UserId = '{userId}'
AND u.TestId = {testId}
AND u.QuestionId = {questionId}
AND u.Type = 'answer'";
			using var results = container.GetItemQueryIterator<int>(query);

			var count = 0;
			while (results.HasMoreResults)
				count = (await results.ReadNextAsync()).Single();

			return count != 0;
		}

		public async Task<bool> IsTestCompleted(Guid userId, int testId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);
			var query = @$"SELECT VALUE (u.IsTestCompleted)
FROM users u
WHERE u.UserId = '{userId}'
AND u.TestId = {testId}
AND u.Type = 'testResult'";
			using var results = container.GetItemQueryIterator<bool>(query);

			bool? res = null;
			while (results.HasMoreResults)
				res = (await results.ReadNextAsync()).Single();

			return res.Value;
		}

		public async Task<int> ReadTestResult(Guid userId, int testId)
		{

			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);

			var query = @$"SELECT  *
FROM users u
WHERE u.UserId = '{userId}'
AND u.TestId = {testId}
AND u.Type = 'testResult'";
			using var results = container.GetItemQueryIterator<UserTestResultDB>(query);

			UserTestResultDB user = null;
			while (results.HasMoreResults)
				user = (await results.ReadNextAsync()).SingleOrDefault();

			if (user == null)
				throw new UserNotFoundException($"User with id {userId} not found.");
			if (!user.IsTestCompleted)
				throw new TestNotCompletedException($"User with id {userId} has not completed test {testId} yet.");

			return user.CorrectAnswerCount;
		}

	}
}
