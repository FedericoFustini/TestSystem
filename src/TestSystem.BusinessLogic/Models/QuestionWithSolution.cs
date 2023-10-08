namespace TestSystem.BusinessLogic.Models
{
	public class QuestionWithSolution : QuestionBase
	{
		public IEnumerable<Answer> Answers { get; set; }
	}
}
