using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.BusinessLogic
{
	internal class DatabaseInitializationManager : IDatabaseInitializationManager
	{
		private readonly IDatabaseStructureRepository _databaseStructureRepository;
		private readonly ILogger<DatabaseInitializationManager> _logger;

		public DatabaseInitializationManager(IDatabaseStructureRepository databaseStructureRepository, ILogger<DatabaseInitializationManager> logger)
		{
			_databaseStructureRepository = databaseStructureRepository;
			_logger = logger;
		}

		public async Task CreateDatabase()
		{
			await _databaseStructureRepository.CreateDatabase();
		}

		public async Task DeleteDatabase()
		{
			await _databaseStructureRepository.DeleteDatabase();
		}

		public async Task PopulateDatabase()
		{
			if (!await _databaseStructureRepository.IsEmpty())
				return;

			var random = new Random(DateTime.Now.Second);
			var testsNumber = random.Next(3, 10);
			var tests = new List<Test>();
			foreach (var testId in Enumerable.Range(1, testsNumber))
				tests.Add(GenerateTest(testId, random));

			_logger.LogInformation("Start inserting {testsCount} tests.", tests.Count);
			await _databaseStructureRepository.PopulateDatabase(tests);
		}

		private Test GenerateTest(int testId, Random random)
		{
			// 100 is an example for populate the db. the number of question can be unbounded and can easily added into db with the used structure
			var questionsNumber = random.Next(10, 100);
			var questions = new List<QuestionWithSolution>();

			foreach (var questionId in Enumerable.Range(1, questionsNumber))
				questions.Add(GenerateQuestion(questionId, random));

			var testQuestionCount = random.Next(3, Math.Min(questionsNumber, 10)); //number of question for a single test

			_logger.LogInformation("Generated test with {questionCount} question and {possibleQuestionsCount} possible questions.", testQuestionCount, questions.Count);
			return new Test()
			{
				Id = testId,
				Name = $"Test {testId}",
				QuestionCount = testQuestionCount,
				PossibleQuestions = questions,
			};
		}

		private QuestionWithSolution GenerateQuestion(int questionId, Random random)
		{

			var answersNumber = random.Next(2, 10); //10 possible answer is only for example purpose. it can be any number.
			var answers = new List<Answer>();

			foreach (var answerId in Enumerable.Range(1, answersNumber))
				answers.Add(new Answer()
				{
					AnswerId = answerId,
					IsCorrect = false,
					Text = $"Answer number {answerId}"
				});

			var correctAnswerId = random.Next(0, answersNumber - 1);
			answers[correctAnswerId].IsCorrect = true;

			return new QuestionWithSolution()
			{
				Id = questionId,
				Text = $"Question number {questionId} ?",
				Answers = answers
			};
		}
	}
}
