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
	public class CompanyConfig : BaseEntityConfig<Company>, IEntityTypeConfiguration<Company>
	{
		public override void Configure(EntityTypeBuilder<Company> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);

			builder.HasMany(x => x.Personnels).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);

			base.Configure(builder);
		}
	}
}
