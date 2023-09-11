using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Models
{
	public class Test
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int QuestionCount { get; set; }
		public IEnumerable<QuestionWithSolution> PossibleQuestions { get; set; }
	}


	public class QuestionWithSolution : QuestionBase
	{
		public IEnumerable<Answer> Answers { get; set; }
	}

	public class Answer : AnswerBase
	{
		public bool IsCorrect { get; set; }
	}
}
