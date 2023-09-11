using TestSystem.BusinessLogic.Models;

namespace TestSystem.Database.Cosmos.Models.testsContainer
{
	internal class QuestionDB
	{
		public QuestionDB()
		{
		}

		public QuestionDB(QuestionWithSolution question, int testId)
		{
			id = Guid.NewGuid();
			TestId = testId;
			QuestionId = question.Id;
			Type = "question";
			Text = question.Text;
			Answers = question.Answers.Select(x => new AnswerDB(x));
		}

		public Guid id { get; set; }
		public int TestId { get; set; }
		public int QuestionId { get; set; }
		public string Type { get; set; } //question
		public string Text { get; set; }
		public IEnumerable<AnswerDB> Answers { get; set; }
	}
}
