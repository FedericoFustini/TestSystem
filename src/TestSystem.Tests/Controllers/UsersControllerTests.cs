using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.Controllers;
using TestSystem.Models;

namespace TestSystem.Tests.Controllers
{
	public class UsersControllerTests
	{
		private Mock<IUsersManager> _usersManager;
		private readonly ILogger<UsersController> _logger = NullLogger<UsersController>.Instance;

		[SetUp]
		public void Setup()
		{
			_usersManager = new Mock<IUsersManager>(MockBehavior.Strict);
		}

		[TearDown]
		public void Teardown()
		{
			_usersManager.VerifyAll();
		}

		[Test]
		public async Task ReadTestResult_UserNotFound_404()
		{
			var userId = Guid.NewGuid();
			var testId = 5;
			var ex = new UserNotFoundException("");
			_usersManager.Setup(x => x.ReadTestResult(userId, testId)).ThrowsAsync(ex);
			var manager = new UsersController(_usersManager.Object, _logger);

			var res = (await manager.ReadTestResult(userId, testId)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
		}

		[Test]
		public async Task ReadTestResult_TestNotCompleted_409()
		{
			var userId = Guid.NewGuid();
			var testId = 5;
			var ex = new TestNotCompletedException("");
			_usersManager.Setup(x => x.ReadTestResult(userId, testId)).ThrowsAsync(ex);
			var manager = new UsersController(_usersManager.Object, _logger);

			var res = (await manager.ReadTestResult(userId, testId)) as ObjectResult;

			Assert.That(res.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
		}
	}
}
