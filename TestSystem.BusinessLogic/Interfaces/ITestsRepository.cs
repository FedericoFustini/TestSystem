using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface ITestsRepository
	{
		Task<bool> ExistTest(int testId);
		Task<TestQuestionsIds> ReadQuestionsIds(int testId);
		Task<IEnumerable<QuestionWithSolution>> ReadTestQuestions(int testId, IEnumerable<int> idsToRetrieve);
		Task<IEnumerable<TestName>> ReadTestsNames();
		Task<int> ReadQuestionsCount(int testId);
		Task<bool> Exist(int testId);
		Task InsertAnswer(UpdateTest updateTest);
	}
}
