using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic.Tests
{
	public class UsersManagerTests
	{
		private Mock<IUsersRepository> _usersRepository;
		private readonly ILogger<UsersManager> _logger = NullLogger<UsersManager>.Instance;

		[SetUp]
		public void Setup()
		{
			_usersRepository = new Mock<IUsersRepository>(MockBehavior.Strict);
		}

		[TearDown]
		public void Teardown()
		{
			_usersRepository.VerifyAll();
		}

		[Test]
		public void UpdateTest_TestNotFound_Throw()
		{
			var testId = 7;
			var userId = Guid.NewGuid();
			_usersRepository.Setup(x => x.ReadTestResult(userId, testId)).ReturnsAsync(4);
			var manager = new UsersManager(_usersRepository.Object, _logger);


			Assert.DoesNotThrowAsync(async () => await manager.ReadTestResult(userId, testId));
		}

	}
}
