using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Models
{
	public class UpdateTest
	{
		public int TestId { get; init; }
		public Guid UserId { get; init; }
		public int QuestionId { get; init; }
		public int AnswerId { get; init; }
	}
}
