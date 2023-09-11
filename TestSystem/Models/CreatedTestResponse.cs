using TestSystem.BusinessLogic.Models;

namespace TestSystem.Models
{
	public class CreatedTestResponse
	{
		public CreatedTestResponse(CreatedTest createdTest)
		{
			UserId = createdTest.UserId;
			Questions = createdTest.Questions.Select(x => new QuestionResponse(x));
		}

		public IEnumerable<QuestionResponse> Questions { get; set; }
		public Guid UserId { get; set; }
	}

	public class QuestionResponse
	{
		public QuestionResponse(Question question)
		{
			Id = question.Id;
			Text = question.Text;
			Answers = question.Answers.Select(x => new AnswerResponse(x));
		}

		public int Id { get; set; }
		public string Text { get; set; }
		public IEnumerable<AnswerResponse> Answers { get; set; }
	}

	public class AnswerResponse
	{
		public AnswerResponse(AnswerBase answer)
		{
			Id = answer.AnswerId;
			Text = answer.Text;
		}

		public int Id { get; set; }
		public string Text { get; set; }
	}
}

