using TestSystem.BusinessLogic.Models;

namespace TestSystem.Database.Cosmos.Models.testsContainer
{
	internal class UserAnswerDB
	{
		public UserAnswerDB() { }
		public UserAnswerDB(UpdateTest updateTest, Answer answer)
		{
			id = Guid.NewGuid();
			TestId = updateTest.TestId;
			QuestionId = updateTest.QuestionId;
			Type = "answer";
			UserId = updateTest.UserId;
			Answer = new AnswerDB()
			{
				AnswerId = updateTest.AnswerId,
				IsCorrect = answer.IsCorrect,
				Text = answer.Text,
			};
		}


		public Guid id { get; set; }
		public int TestId { get; set; }
		public int QuestionId { get; set; }
		public string Type { get; set; } //answer
		public Guid UserId { get; set; }
		public AnswerDB Answer { get; set; }
	}
}
