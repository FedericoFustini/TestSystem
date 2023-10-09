using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Controllers;
using TestSystem.Models;

namespace TestSystem.Tests.Controllers
{
	public class TestsControllerTests
	{
		private Mock<ITestsManager> _testsManager;
		private readonly ILogger<TestsController> _logger = NullLogger<TestsController>.Instance;

		[SetUp]
		public void Setup()
		{
			_testsManager = new Mock<ITestsManager>(MockBehavior.Strict);
		}

		[TearDown]
		public void Teardown()
		{
			_testsManager.VerifyAll();
		}

		[Test]
		public async Task CreateTest_TestNotFound_404()
		{
			var request = new CreateTestRequest()
			{
				Username = "testUser",
				TestId = 4,
			};
			var ex = new TestNotFoundException("");
			_testsManager.Setup(x => x.CreateTest(request.Username, request.TestId.Value)).ThrowsAsync(ex);
			var manager = new TestsController(_testsManager.Object, _logger);

			var res = (await manager.CreateTest(request)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task UpdateTest_AnswerConflict_409()
		{
			var request = new UpdateTestRequest()
			{
				AnswerId = 1,
				TestId = 4,
				QuestionId = 1,
				UserId = Guid.NewGuid()
			};
			var ex = new AnswerConflictException("");
			_testsManager.Setup(x => x.UpdateTest(It.Is<UpdateTest>(x =>
					x.TestId == request.TestId
					&& x.UserId == request.UserId
					&& x.AnswerId == request.AnswerId
					&& x.QuestionId == request.QuestionId)))
				.ThrowsAsync(ex);
			var manager = new TestsController(_testsManager.Object, _logger);


			var res = (await manager.UpdateTest(request)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
		}

		[Test]
		public async Task UpdateTest_UserNotFound_404()
		{
			var request = new UpdateTestRequest()
			{
				AnswerId = 1,
				TestId = 4,
				QuestionId = 1,
				UserId = Guid.NewGuid()
			};
			var ex = new UserNotFoundException("");
			_testsManager.Setup(x => x.UpdateTest(It.Is<UpdateTest>(x =>
					x.TestId == request.TestId
					&& x.UserId == request.UserId
					&& x.AnswerId == request.AnswerId
					&& x.QuestionId == request.QuestionId)))
				.ThrowsAsync(ex);
			var manager = new TestsController(_testsManager.Object, _logger);

			var res = (await manager.UpdateTest(request)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}


		[Test]
		public async Task UpdateTest_TestNotFound_404()
		{
			var request = new UpdateTestRequest()
			{
				AnswerId = 1,
				TestId = 4,
				QuestionId = 1,
				UserId = Guid.NewGuid()
			};
			var ex = new TestNotFoundException("");
			_testsManager.Setup(x => x.UpdateTest(It.Is<UpdateTest>(x =>
					x.TestId == request.TestId
					&& x.UserId == request.UserId
					&& x.AnswerId == request.AnswerId
					&& x.QuestionId == request.QuestionId)))
				.ThrowsAsync(ex);
			var manager = new TestsController(_testsManager.Object, _logger);

			var res = (await manager.UpdateTest(request)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}
	}
}
