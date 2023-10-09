using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestSystem.Database.SqlServer.Entities
{
	[Table("Users", Schema = "Users")]
	internal class DbUser
	{
		[Key]
		public Guid Id { get; set; } //pay attention, this should be NEWSEQUENTIALID() and not NEWID()
		[MaxLength(100)]
		public string Name { get; set; }


		public virtual ICollection<DbUserAnswer> UsersAnswers { get; set; }
	}

	internal class DbUsersConfiguration : IEntityTypeConfiguration<DbUser>
	{
		public void Configure(EntityTypeBuilder<DbUser> builder)
		{
			builder
			.Property(b => b.Name)
			.IsRequired();

			builder.Property(b => b.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
		}
	}
}
