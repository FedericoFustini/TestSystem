using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.Database.Cosmos.Models.usersContainer
{
	internal class UserTestResultDB
	{
		public Guid id { get; set; }
		public Guid UserId { get; set; }
		public int TestId { get; set; }
		public string Username { get; set; }
		public string Type { get; set; } //testResult
		public int CorrectAnswerCount { get; set; }
		public int TotalAnswerCount { get; set; }
		public bool IsTestCompleted { get; set; }

		public UserTestResultDB()
		{
		}

		public UserTestResultDB(Guid userId, string username, int testId)
		{
			id = Guid.NewGuid();
			UserId = userId;
			TestId = testId;
			Username = username;
			Type = "testResult";
			CorrectAnswerCount = 0;
			TotalAnswerCount = 0;
			IsTestCompleted = false;
		}
	}
}
