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
	public class AbsenceConfig : BaseEntityConfig<Absence>,IEntityTypeConfiguration<Absence>
	{
		public override void Configure(EntityTypeBuilder<Absence> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x=>x.StartDate).IsRequired(true);
			builder.Property(x=>x.EndDate).IsRequired(true);
			builder.Property(x=>x.AbsenceDuration).IsRequired(true);
			builder.Property(x=>x.LeaveTypes).IsRequired(true);
			builder.Property(x=>x.Reason).IsRequired(true).HasMaxLength(300);
			builder.Property(x=>x.Condition).IsRequired(true);
			builder.Property(x=>x.PersonnelId).IsRequired(true);
			builder.Property(x=>x.AbsenceDuration).HasColumnType("bigint").IsRequired();

			builder.HasOne(x=>x.Personnel).WithMany(x=>x.Absences).HasForeignKey(x=>x.PersonnelId);

			base.Configure(builder);
		}
	}
}
