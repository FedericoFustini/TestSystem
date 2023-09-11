using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic.Tests
{

	public class TestsManagerTests
	{
		private Mock<ITestsRepository> _testsRepository;
		private Mock<IUsersRepository> _usersRepository;
		private readonly ILogger<TestsManager> _logger = NullLogger<TestsManager>.Instance;

		[SetUp]
		public void Setup()
		{
			_testsRepository = new Mock<ITestsRepository>(MockBehavior.Strict);
			_usersRepository = new Mock<IUsersRepository>(MockBehavior.Strict);
		}

		[TearDown]
		public void Teardown()
		{
			_testsRepository.VerifyAll();
			_usersRepository.VerifyAll();
		}

		[Test]
		public void UpdateTest_TestNotFound_Throw()
		{
			var update = new UpdateTest()
			{
				TestId = 100,
			};
			_testsRepository.Setup(x => x.Exist(update.TestId)).ReturnsAsync(false);
			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);


			Assert.ThrowsAsync<TestNotFoundException>(async () => await manager.UpdateTest(update));
		}

		[Test]
		public void UpdateTest_UserNotFound_Throw()
		{
			var update = new UpdateTest()
			{
				TestId = 100,
			};
			_testsRepository.Setup(x => x.Exist(update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.Exist(update.UserId, update.TestId)).ReturnsAsync(false);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);


			Assert.ThrowsAsync<UserNotFoundException>(async () => await manager.UpdateTest(update));
		}

		[Test]
		public void UpdateTest_AnswerExistYet_Throw()
		{
			var update = new UpdateTest()
			{
				TestId = 100,
			};
			_testsRepository.Setup(x => x.Exist(update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.Exist(update.UserId, update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.ExistAnswer(update.UserId, update.TestId, update.QuestionId)).ReturnsAsync(true);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);


			Assert.ThrowsAsync<AnswerConflictException>(async () => await manager.UpdateTest(update));
		}

		[Test]
		public void UpdateTest_TestCompletedYet_Throw()
		{
			var update = new UpdateTest()
			{
				TestId = 100,
			};
			_testsRepository.Setup(x => x.Exist(update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.Exist(update.UserId, update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.ExistAnswer(update.UserId, update.TestId, update.QuestionId)).ReturnsAsync(false);
			_usersRepository.Setup(x => x.IsTestCompleted(update.UserId, update.TestId)).ReturnsAsync(true);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);


			Assert.ThrowsAsync<AnswerConflictException>(async () => await manager.UpdateTest(update));
		}


		[Test]
		public void UpdateTest_HappyPath_Ok()
		{
			var update = new UpdateTest()
			{
				TestId = 1,
				UserId = Guid.NewGuid(),
				AnswerId = 2,
				QuestionId = 3
			};
			_testsRepository.Setup(x => x.Exist(update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.Exist(update.UserId, update.TestId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.ExistAnswer(update.UserId, update.TestId, update.QuestionId)).ReturnsAsync(false);
			_usersRepository.Setup(x => x.IsTestCompleted(update.UserId, update.TestId)).ReturnsAsync(false);
			_testsRepository.Setup(x => x.InsertAnswer(update)).Returns(Task.CompletedTask);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);


			Assert.DoesNotThrowAsync(async () => await manager.UpdateTest(update));
		}

		[Test]
		public async Task CreateTest_HappyPath_Ok()
		{
			var username = "user";
			var testId = 99;
			var ids = new TestQuestionsIds
			{
				QuestionsIds = new[] { 1, 2, 3 },
				TestQuestionsCount = 2
			};
			var solutions = new[]
			{
				new QuestionWithSolution
				{
					Id = 1,
					Text = "question 1?",
					Answers = new []
					{
						new Answer { Text = "answer1", AnswerId = 1, IsCorrect = true },
						new Answer { Text = "answer2", AnswerId = 2, IsCorrect = false },
					},
				},
				new QuestionWithSolution
				{
					Id = 2,
					Text = "question 2?",
					Answers = new []
					{
						new Answer { Text = "answer1", AnswerId = 1, IsCorrect = false },
						new Answer { Text = "answer2", AnswerId = 2, IsCorrect = true },
					},
				},
			};
			_testsRepository.Setup(x => x.ExistTest(testId)).ReturnsAsync(true);
			_usersRepository.Setup(x => x.CreateUser(It.IsAny<Guid>(), username, testId)).Returns(Task.CompletedTask);
			_testsRepository.Setup(x => x.ReadQuestionsIds(testId)).ReturnsAsync(ids);
			_testsRepository.Setup(x => x.ReadTestQuestions(testId, It.IsAny<IEnumerable<int>>())).ReturnsAsync(solutions);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);

			var res = await manager.CreateTest(username, testId);

			Assert.That(res.Questions.Count(), Is.EqualTo(2));
		}



		[Test]
		public async Task CreateTest_TestoNotFound_Throw()
		{
			var username = "user";
			var testId = 99;
			var ids = new TestQuestionsIds
			{
				QuestionsIds = new[] { 1, 2, 3 },
				TestQuestionsCount = 2
			};
			_testsRepository.Setup(x => x.ExistTest(testId)).ReturnsAsync(false);

			var manager = new TestsManager(_testsRepository.Object, _usersRepository.Object, _logger);

			Assert.ThrowsAsync<TestNotFoundException>(async () => await manager.CreateTest(username, testId));
		}



	}
}
