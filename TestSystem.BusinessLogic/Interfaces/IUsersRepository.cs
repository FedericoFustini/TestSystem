using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface IUsersRepository
	{
		Task CreateUser(Guid userId, string username, int testId);
		Task<bool> Exist(Guid userId, int testId);
		Task<bool> ExistAnswer(Guid userId, int testId, int questionId);
		Task<bool> IsTestCompleted(Guid userId, int testId);
		Task<int> ReadTestResult(Guid userId, int testId);
	}
}
