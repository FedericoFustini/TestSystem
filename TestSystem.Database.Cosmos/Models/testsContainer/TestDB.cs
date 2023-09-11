using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.Database.Cosmos.Models.testsContainer
{
	internal class TestDB
	{
		public TestDB()
		{
		}

		public TestDB(TestToGenerate test,int testId)
		{
			id = Guid.NewGuid();
			TestId = testId;
			QuestionId = 0;
			Type = "test";
			Name = test.Name;
			QuestionCount = test.QuestionCount;
		}

		public Guid id { get; set; }
		public int TestId { get; set; }
		public int QuestionId { get; set; }
		public string Type { get; set; } //test
		public string Name { get; set; }
		public int QuestionCount { get; set; }
	}
}
