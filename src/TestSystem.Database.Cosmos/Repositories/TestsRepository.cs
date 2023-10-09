using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Database.Cosmos.Models.testsContainer;
using TestSystem.Database.Cosmos.Models.usersContainer;

namespace TestSystem.Database.Cosmos.Repositories
{
	internal class TestsRepository : ITestsRepository
	{
		private readonly CosmosClient _cosmosClient;

		public TestsRepository(CosmosClient cosmosClient)
		{
			_cosmosClient = cosmosClient;
		}

		public async Task<TestQuestionsIds> ReadQuestionsIds(int testId)
		{
			var questionCount = ReadQuestionsCount(testId);
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var query = new QueryDefinition(@"SELECT VALUE t.QuestionId
FROM tests t
WHERE t.TestId = @testId
AND t.Type = 'question'")
				.WithParameter("@testId", testId);

			using var results = container.GetItemQueryIterator<int>(query);
			var ids = new List<int>();
			while (results.HasMoreResults)
			{

				var resultsPage = await results.ReadNextAsync();
				foreach (var result in resultsPage)
					ids.Add(result);
			}
			return new TestQuestionsIds()
			{
				QuestionsIds = ids,
				TestQuestionsCount = await questionCount
			};
		}

		public async Task<int> ReadQuestionsCount(int testId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var query = new QueryDefinition(@"SELECT VALUE t.QuestionCount
FROM tests t
WHERE t.TestId = @testId
AND t.QuestionId = 0
AND t.Type = 'test'")
	.WithParameter("@testId", testId);

			using var results = container.GetItemQueryIterator<int>(query);

			int? count = null;
			while (results.HasMoreResults)
				count = (await results.ReadNextAsync()).Single();
			return count.Value;
		}

		public async Task<IEnumerable<QuestionWithSolution>> ReadTestQuestions(int testId, IEnumerable<int> idsToRetrieve)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var query = new QueryDefinition(@"SELECT VALUE { Id: t.QuestionId, Text: t.Text, Answers: t.Answers  }
FROM tests t
WHERE t.TestId = @testId
AND t.QuestionId IN" + $" ({String.Join(", ", idsToRetrieve)}) " +
@"AND t.Type = 'question'")
				.WithParameter("@testId", testId);

			using var results = container.GetItemQueryIterator<QuestionWithSolution>(query);

			var questions = new List<QuestionWithSolution>();
			while (results.HasMoreResults)
			{
				var resultsPage = await results.ReadNextAsync();
				foreach (var result in resultsPage)
					questions.Add(result);
			}

			return questions;
		}

		public async Task<IEnumerable<TestName>> ReadTestsNames()
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var query = new QueryDefinition(@"SELECT DISTINCT VALUE {Id: t.TestId, Name: t.Name}
FROM tests t
WHERE t.QuestionId = 0
AND t.Type = 'test'");
			using var results = container.GetItemQueryIterator<TestName>(query);

			var res = new List<TestName>();
			while (results.HasMoreResults)
			{
				var resultsPage = await results.ReadNextAsync();
				foreach (var result in resultsPage)
					res.Add(result);
			}
			return res;
		}

		public async Task<bool> ExistTest(int testId)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);
			var query = @$"SELECT VALUE COUNT(t.id)
FROM tests t
WHERE t.TestId = {testId}
AND t.QuestionId = 0
AND t.Type = 'test'";
			using var results = container.GetItemQueryIterator<int>(query);

			int? count = null;
			while (results.HasMoreResults)
				count = (await results.ReadNextAsync()).Single();

			return count.Value != 0;

		}

		public async Task InsertUserAnswer(UpdateTest updateTest)
		{
			var answerWithSolution = await InsertAnswerIntoTests(updateTest);
			await InsertAnswerIntoUsers(updateTest, answerWithSolution);
		}

		private async Task InsertAnswerIntoUsers(UpdateTest updateTest, Answer answerWithSolution)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.USER_CONTAINER);

			var testItem = new UserAnswerDB(updateTest, answerWithSolution);
			_ = await container.CreateItemAsync(testItem);


			var query = @$"SELECT *
FROM users u
WHERE u.TestId = {updateTest.TestId}
AND u.UserId = '{updateTest.UserId}'
AND u.Type = 'testResult'";
			using var results = container.GetItemQueryIterator<UserTestResultDB>(query);

			UserTestResultDB res = null;
			while (results.HasMoreResults)
				res = (await results.ReadNextAsync()).Single();


			var count = await ReadQuestionsCount(updateTest.TestId);

			res.TotalAnswerCount++;
			res.IsTestCompleted = count == res.TotalAnswerCount;
			if (answerWithSolution.IsCorrect)
				res.CorrectAnswerCount++;

			await container.UpsertItemAsync(res);
		}

		private async Task<Answer> InsertAnswerIntoTests(UpdateTest updateTest)
		{
			var container = _cosmosClient.GetContainer(DBConstants.TEST_DATABASE, DBConstants.TEST_CONTAINER);

			var question = (await ReadTestQuestions(updateTest.TestId, new[] { updateTest.QuestionId })).Single();
			var answer = question.Answers.First(x => x.AnswerId == updateTest.AnswerId);
			var testItem = new UserAnswerDB(updateTest, answer);

			_ = await container.CreateItemAsync(testItem);
			return answer;
		}
	}
}
