namespace TestSystem.Models
{
	public class CorrectAnswerResponse
	{
		public CorrectAnswerResponse(int count)
		{
			CorrectAnswerCount = count;
		}

		public int CorrectAnswerCount { get; init; }
	}
}
