using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic
{
	internal class UsersManager : IUsersManager
	{
		private readonly IUsersRepository _usersRepository;
		private readonly ILogger<UsersManager> _logger;

		public UsersManager(IUsersRepository usersRepository, ILogger<UsersManager> logger)
		{
			_usersRepository = usersRepository;
			_logger = logger;
		}

		public async Task<int> ReadTestResult(Guid userId, int testId)
		{
			return await _usersRepository.ReadTestResult(userId, testId);
		}
	}
}
