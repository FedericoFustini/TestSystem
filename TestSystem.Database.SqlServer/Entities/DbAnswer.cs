using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestSystem.Database.SqlServer.Entities
{
	[Table("Answers", Schema = "Tests")]
	internal class DbAnswer
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int QuestionId { get; set; }
		[Required, MaxLength(500)]
		public string Text { get; set; }
		[Required]
		public bool IsCorrect { get; set; }

		public virtual DbQuestion Question { get; set; }
		public virtual ICollection<DbUserAnswer> UsersAnswers { get; set; }

	}

	internal class DbAnswerConfiguration : IEntityTypeConfiguration<DbAnswer>
	{
		public void Configure(EntityTypeBuilder<DbAnswer> builder)
		{
			builder.Property(x => x.QuestionId).IsRequired();
			builder.Property(x => x.Text).IsRequired();

			builder.HasOne(x => x.Question)
				.WithMany(x => x.Answers)
				.HasForeignKey(x => x.QuestionId);
		}
	}

}
