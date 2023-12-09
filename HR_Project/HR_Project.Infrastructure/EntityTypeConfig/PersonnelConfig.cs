using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR_Project.Infrastructure.EntityTypeConfig
{
	public class PersonnelConfig : BaseEntityConfig<Personnel>, IEntityTypeConfiguration<Personnel>
	{
		public override void Configure(EntityTypeBuilder<Personnel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired(true).HasMaxLength(20);
			builder.Property(x => x.Surname).IsRequired(true).HasMaxLength(15);
			builder.Property(x => x.Email).IsRequired(true).HasMaxLength(25);
			builder.Property(x => x.PasswordHash).IsRequired(true);

			

			builder.HasMany(x => x.Absences).WithOne(x => x.Personnel).HasForeignKey(x => x.PersonnelId);

			builder.HasMany(x => x.Advances).WithOne(x => x.Personnel).HasForeignKey(x => x.PersonnelId);

			builder.HasMany(x => x.Personnels).WithOne(x => x.Manager).HasForeignKey(x => x.ManagerId).OnDelete(DeleteBehavior.NoAction);

			base.Configure(builder);
		}
	}
}
