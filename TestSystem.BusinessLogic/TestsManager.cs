using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic
{
	internal class TestsManager : ITestsManager
	{
		private readonly ITestsRepository _testsRepository;
		private readonly IUsersRepository _usersRepository;
		private readonly ILogger<TestsManager> _logger;

		public TestsManager(ITestsRepository testsRepository, IUsersRepository usersRepository, ILogger<TestsManager> logger)
		{
			_testsRepository = testsRepository;
			_usersRepository = usersRepository;
			_logger = logger;
		}

		public async Task<IEnumerable<TestName>> ReadTestsNames()
		{
			return await _testsRepository.ReadTestsNames();
		}

		public async Task<CreatedTest> CreateTest(string username, int testId)
		{
			if (!await _testsRepository.ExistTest(testId))
				throw new TestNotFoundException($"Test with id {testId} was not found.");

			_logger.LogInformation("Creating user {username} for test {testId}", username, testId);

			var userId = await _usersRepository.CreateUser(username, testId);

			var questionsIds = await _testsRepository.ReadQuestionsIds(testId);
			var idsToRetrieve = questionsIds.QuestionsIds
				.OrderBy(x => Guid.NewGuid())
				.Take(questionsIds.TestQuestionsCount)
				.ToList();

			var questions = await _testsRepository.ReadTestQuestions(testId, idsToRetrieve);

			return new CreatedTest
			{
				UserId = userId,
				Questions = questions.Select(x => new Question
				{
					Id = x.Id,
					Text = x.Text,
					Answers = x.Answers.Select( y => new AnswerBase
					{
						Text = y.Text,
						AnswerId = y.AnswerId,
					})
				})
			};
		}

		public async Task UpdateTest(UpdateTest updateTest)
		{
			if (!await _testsRepository.ExistTest(updateTest.TestId))
				throw new TestNotFoundException($"Test with {updateTest.TestId} not found.");
			if (!await _usersRepository.Exist(updateTest.UserId, updateTest.TestId))
				throw new UserNotFoundException($"User with {updateTest.UserId} not found.");
			if (await _usersRepository.ExistAnswer(updateTest.UserId, updateTest.TestId, updateTest.QuestionId))
				throw new AnswerConflictException($"User with {updateTest.UserId} has answer to question {updateTest.QuestionId} of test {updateTest.TestId} yet.");
			if (await _usersRepository.IsTestCompleted(updateTest.UserId, updateTest.TestId))
				throw new AnswerConflictException($"User with {updateTest.UserId} has completed test {updateTest.TestId} yet.");
			await _testsRepository.InsertUserAnswer(updateTest);
		}
	}
}
