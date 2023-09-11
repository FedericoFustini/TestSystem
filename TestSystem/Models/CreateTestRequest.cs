using System.ComponentModel.DataAnnotations;

namespace TestSystem.Models
{
	public class CreateTestRequest
	{
		[Required]
		public string Username { get; set; }
		[Required, Range(1, int.MaxValue)]
		public int? TestId { get; set; }
	}

	public class AnswerRequest
	{
		[Required]
		public int? TestId { get; set; }

	}
}
