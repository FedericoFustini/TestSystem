using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSystem.Database.SqlServer.Entities;

namespace TestSystem.Database.SqlServer
{
	internal class TestSystemContext : DbContext
	{
		public TestSystemContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<DbUser> Users { get; set; }
		public DbSet<DbTest> Tests { get; set; }
		public DbSet<DbQuestion> Questions { get; set; }
		public DbSet<DbAnswer> Answers { get; set; }
		public DbSet<DbUserAnswer> UsersAnswers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestSystemContext).Assembly);
		}

	}
}
