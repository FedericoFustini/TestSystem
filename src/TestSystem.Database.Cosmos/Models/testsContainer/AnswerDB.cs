using TestSystem.BusinessLogic.Models;

namespace TestSystem.Database.Cosmos.Models.testsContainer
{
	internal class AnswerDB
	{
		public AnswerDB()
		{
		}

		public AnswerDB(AnswerToGenerate answer, int answerId)
		{
			AnswerId = answerId;
			Text = answer.Text;
			IsCorrect = answer.IsCorrect;
		}

		public int AnswerId { get; set; }
		public string Text { get; set; }
		public bool IsCorrect { get; set; }
	}
}
