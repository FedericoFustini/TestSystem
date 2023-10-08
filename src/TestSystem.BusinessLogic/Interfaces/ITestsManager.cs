using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface ITestsManager
	{
		Task<IEnumerable<TestName>> ReadTestsNames();
		Task<CreatedTest> CreateTest(string username, int testId);
		Task UpdateTest(UpdateTest updateTest);
	}
}
