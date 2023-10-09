using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using TestSystem.BusinessLogic.Exceptions;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.Database.SqlServer.Entities;

namespace TestSystem.Database.SqlServer.Repositories
{
	internal class UsersRepository : IUsersRepository
	{
		private readonly TestSystemContext _dbContext;

		public UsersRepository(TestSystemContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<Guid> CreateUser(string username, int testId)
		{
			var entity = _dbContext.Users.Add(new DbUser
			{
				Name = username,
			});
			await _dbContext.SaveChangesAsync();
			return entity.Entity.Id;
		}

		public async Task<bool> Exist(Guid userId, int testId)
		{
			return await _dbContext.Users.AnyAsync(x => x.Id == userId);
		}

		public async Task<bool> ExistAnswer(Guid userId, int testId, int questionId)
		{
			return await _dbContext.UsersAnswers.AnyAsync(x => x.UserId == userId && x.Answer.QuestionId == questionId);
		}

		public async Task<bool> IsTestCompleted(Guid userId, int testId)
		{
			var questionCount = await _dbContext.Tests.Where(x => x.Id == testId).Select(x => x.QuestionCount).FirstAsync();
			var answerCount = await _dbContext.UsersAnswers.CountAsync(x => x.UserId == userId && x.Answer.Question.TestId == testId);
			return questionCount == answerCount;
		}

		public async Task<int> ReadTestResult(Guid userId, int testId)
		{
			var user = await _dbContext.Users.CountAsync(x => x.Id == userId);
			if (user == 0)
				throw new UserNotFoundException($"User with id {userId} not found.");
			if (!await IsTestCompleted(userId, testId))
				throw new TestNotCompletedException($"User with id {userId} has not completed test {testId} yet.");

			return await _dbContext.UsersAnswers.CountAsync(x => x.UserId == userId && x.Answer.IsCorrect && x.Answer.Question.TestId == testId);
		}
	}
}
