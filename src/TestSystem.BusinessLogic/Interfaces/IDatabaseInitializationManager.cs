using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Interfaces
{
	public interface IDatabaseInitializationManager
	{
		public Task DeleteDatabase();
		public Task CreateDatabase();
		public Task PopulateDatabase();
	}
}
