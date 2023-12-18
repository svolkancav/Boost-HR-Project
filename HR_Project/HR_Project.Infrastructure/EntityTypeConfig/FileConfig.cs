using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = HR_Project.Domain.Entities.Concrete.FileEntities.File;

namespace HR_Project.Infrastructure.EntityTypeConfig
{
	public class FileConfig : BaseEntityConfig<File>, IEntityTypeConfiguration<File>
	{
		public override void Configure(EntityTypeBuilder<File> builder)
		{
			builder.ToTable("Files").
				HasDiscriminator<string>("FileType").
				HasValue<PersonnelPicture>(nameof(PersonnelPicture)).
				HasValue<CostPicture>(nameof(CostPicture));

		}
	}
}
