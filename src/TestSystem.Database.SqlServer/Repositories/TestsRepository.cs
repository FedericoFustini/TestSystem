using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Database.SqlServer.Entities;

namespace TestSystem.Database.SqlServer.Repositories
{
	internal class TestsRepository : ITestsRepository
	{
		private readonly TestSystemContext _dbContext;

		public TestsRepository(TestSystemContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> ExistTest(int testId)
		{
			return await _dbContext.Tests.AnyAsync(x => x.Id == testId);
		}

		public async Task InsertUserAnswer(UpdateTest updateTest)
		{
			var isConsistent = await _dbContext.Answers
				.AnyAsync(x => x.Id == updateTest.AnswerId
					&& x.QuestionId == updateTest.QuestionId
					&& x.Question.TestId == updateTest.TestId);
			if (!isConsistent)
				throw new AnswerConflictException($"Answer {updateTest.AnswerId} is not an answer for question {updateTest.QuestionId} for test {updateTest.TestId}.");

			_dbContext.UsersAnswers.Add(new DbUserAnswer
			{
				AnswerId = updateTest.AnswerId,
				UserId = updateTest.UserId,
			});
			await _dbContext.SaveChangesAsync();
		}

		public async Task<int> ReadQuestionsCount(int testId)
		{
			return await _dbContext.Questions.CountAsync(x => x.TestId == testId);
		}

		public async Task<TestQuestionsIds> ReadQuestionsIds(int testId)
		{
			var ids = await _dbContext.Questions.Where(x => x.TestId == testId).Select(x => x.Id).ToListAsync();
			var count = await _dbContext.Tests.Where(x => x.Id == testId).Select(x => x.QuestionCount).FirstAsync();
			return new TestQuestionsIds
			{
				QuestionsIds = ids,
				TestQuestionsCount = count,
			};
		}

		public async Task<IEnumerable<QuestionWithSolution>> ReadTestQuestions(int testId, IEnumerable<int> idsToRetrieve)
		{
			return await _dbContext.Questions
				.Where(x => idsToRetrieve.Contains(x.Id))
				.Select(x => new QuestionWithSolution
				{
					Id = x.Id,
					Text = x.Text,
					Answers = x.Answers.Select(y => new Answer
					{
						AnswerId = y.Id,
						Text = y.Text,
						IsCorrect = y.IsCorrect,
					})
				}).ToListAsync();

		}

		public async Task<IEnumerable<TestName>> ReadTestsNames()
		{
			return await _dbContext.Tests.Select(x => new TestName
			{
				Id = x.Id,
				Name = x.Name
			}).ToListAsync();
		}
	}
}
