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
	public class AdvanceConfig : BaseEntityConfig<Advance>, IEntityTypeConfiguration<Advance>
	{
		public override void Configure(EntityTypeBuilder<Advance> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.LastPaidDate).IsRequired(true);
			builder.Property(x => x.Amount).IsRequired(true).HasColumnType("decimal(18,2)");
			builder.Property(x => x.Reason).IsRequired(true).HasMaxLength(300);
			builder.Property(x => x.Condition).IsRequired(true);
			builder.Property(x => x.PersonnelId).IsRequired(true);

			builder.HasOne(x => x.Personnel).WithMany(x => x.Advances).HasForeignKey(x => x.PersonnelId);

			base.Configure(builder);
		}
	}
}
