using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestSystem.BusinessLogic.Models;

namespace TestSystem.Database.SqlServer.Entities
{
	[Table("Questions", Schema = "Tests")]
	internal class DbQuestion
	{
		[Key]
		public int Id { get; set; }
		public int TestId { get; set; }
		[MaxLength(500)]
		public string Text { get; set; }

		public virtual DbTest Test { get; set; }
		public virtual ICollection<DbAnswer> Answers { get; set; }
	}

	internal class DbQuestionConfiguration : IEntityTypeConfiguration<DbQuestion>
	{
		public void Configure(EntityTypeBuilder<DbQuestion> builder)
		{
			builder.Property(x => x.TestId).IsRequired();
			builder.Property(x => x.Text).IsRequired();

			builder.HasOne(x => x.Test)
				.WithMany(x => x.Questions)
				.HasForeignKey(x => x.TestId);
		}
	}

}
