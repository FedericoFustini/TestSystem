using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSystem.BusinessLogic.Models
{
	public class TestToGenerate
	{
		public string Name { get; set; }
		public int QuestionCount { get; set; }
		public IEnumerable<QuestionToGenerate> PossibleQuestions { get; set; }
	}

	public class QuestionToGenerate
	{
		public string Text { get; set; }

		public IEnumerable<AnswerToGenerate> Answers { get; set; }

	}

	public class AnswerToGenerate
	{
		public string Text { get; set; }
		public bool IsCorrect { get; set; }
	}
}
