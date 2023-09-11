using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace TestSystem.Database.SqlServer.Entities
{
	[Table("UsersAnswers", Schema = "Users")]
	internal class DbUserAnswer
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public Guid UserId { get; set; }
		[Required]
		public int AnswerId { get; set; }

		public virtual DbUser User { get; set; }
		public virtual DbAnswer Answer { get; set; }

	}

	internal class DbUserAnswerConfiguration : IEntityTypeConfiguration<DbUserAnswer>
	{
		public void Configure(EntityTypeBuilder<DbUserAnswer> builder)
		{
			builder.Property(x => x.UserId).IsRequired();
			builder.Property(x => x.AnswerId).IsRequired();

			builder.HasIndex(p => new { p.UserId, p.AnswerId })
				.IsUnique();

			builder.HasOne(x => x.User)
				.WithMany(x => x.UsersAnswers)
				.HasForeignKey(x => x.UserId);
			builder.HasOne(x => x.Answer)
				.WithMany(x => x.UsersAnswers)
				.HasForeignKey(x => x.AnswerId);
		}
	}

}
