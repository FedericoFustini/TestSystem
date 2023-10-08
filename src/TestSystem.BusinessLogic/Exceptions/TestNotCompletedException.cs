using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Exceptions
{
	public class TestNotCompletedException : Exception
	{
		public TestNotCompletedException(string? message) : base(message)
		{
		}
	}
}
