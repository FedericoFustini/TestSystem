using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Database.SqlServer.Entities
{
	[Table("Tests", Schema = "Tests")]
	internal class DbTest
	{
		[Key]
		public int Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		public int QuestionCount { get; set; }

		public virtual ICollection<DbQuestion> Questions { get; set; }

	}

	internal class DbTestsConfiguration : IEntityTypeConfiguration<DbTest>
	{
		public void Configure(EntityTypeBuilder<DbTest> builder)
		{
			builder
				.Property(b => b.Name)
				.IsRequired();

			builder
				.Property(b => b.QuestionCount)
				.IsRequired();

		}
	}

}
