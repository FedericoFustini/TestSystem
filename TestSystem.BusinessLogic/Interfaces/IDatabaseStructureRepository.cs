using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface IDatabaseStructureRepository
	{
		Task CreateDatabase();
		Task DeleteDatabase();
		Task<bool> IsEmpty();
		Task PopulateDatabase(IEnumerable<TestToGenerate> tests);
	}
}
