using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface IUsersManager
	{
		Task<int> ReadTestResult(Guid userId, int testId);
	}
}
