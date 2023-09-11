using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Exceptions
{
	public class TestNotFoundException : Exception
	{
		public TestNotFoundException(string? message) : base(message)
		{
		}
	}
}
