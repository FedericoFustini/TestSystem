using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestSystem.BusinessLogic.Interfaces;
using TestSystem.BusinessLogic.Models;
using TestSystem.Database.SqlServer.Entities;

namespace TestSystem.Database.SqlServer.Repositories
{
	internal class DatabaseStructureRepository : IDatabaseStructureRepository
	{
		private readonly TestSystemContext _dbContext;

		public DatabaseStructureRepository(TestSystemContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task CreateDatabase()
		{
			await _dbContext.Database.EnsureCreatedAsync();
		}

		public async Task DeleteDatabase()
		{
			await _dbContext.Database.EnsureDeletedAsync();
		}

		public async Task<bool> IsEmpty()
		{
			return !await _dbContext.Tests.AnyAsync();
		}

		public async Task PopulateDatabase(IEnumerable<TestToGenerate> tests)
		{
			var dbTests = tests.Select(x => new DbTest
			{
				Name = x.Name,
				QuestionCount = x.QuestionCount,
				Questions = x.PossibleQuestions.Select(y => new DbQuestion
				{
					Text = y.Text,
					Answers = y.Answers.Select(z => new DbAnswer
					{
						IsCorrect = z.IsCorrect,
						Text = z.Text,
					}).ToList(),
				}).ToList()
			});
			_dbContext.Tests.AddRange(dbTests);
			await _dbContext.SaveChangesAsync();
		}
	}
}
