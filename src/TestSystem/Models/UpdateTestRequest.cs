using System.ComponentModel.DataAnnotations;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.Models
{
	public class UpdateTestRequest
	{
		[Required, Range(1, int.MaxValue)]
		public int? TestId { get; set; }
		[Required]
		public Guid? UserId { get; set; }
		[Required, Range(1, int.MaxValue)]
		public int? QuestionId { get; set; }
		[Required, Range(1, int.MaxValue)]
		public int? AnswerId { get; set; }

		public UpdateTest ToBLUpdateTest()
		{
			return new UpdateTest
			{
				AnswerId = AnswerId.Value,
				UserId = UserId.Value,
				QuestionId = QuestionId.Value,
				TestId = TestId.Value
			};
		}
	}
}
