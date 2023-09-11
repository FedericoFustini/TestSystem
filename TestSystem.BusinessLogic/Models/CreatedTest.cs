using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Models
{
	public class CreatedTest
	{
		public IEnumerable<Question> Questions { get; set; }
		public Guid UserId { get; set; }
	}

	public class Question : QuestionBase
	{
		public IEnumerable<AnswerBase> Answers { get; set; }
	}
}
