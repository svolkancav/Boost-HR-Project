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
	public class DepartmentConfig : BaseEntityConfig<Department>, IEntityTypeConfiguration<Department>
	{
		public override void Configure(EntityTypeBuilder<Department> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired(true).HasMaxLength(15);
			builder.Property(x => x.ManagerId).IsRequired(true);
			builder.Property(x => x.CompanyId).IsRequired(true);
			
			builder.HasMany(x => x.Personnels).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.Manager).WithOne().HasForeignKey<Department>(x => x.ManagerId);
			
			builder.HasOne(x => x.Company).WithMany(x => x.Departments).HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.NoAction);

			base.Configure(builder);
		}
	}
}
