using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR_Project.Infrastructure.EntityTypeConfig
{
	public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T :class, IBaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{
			builder.Property(x => x.CreatedDate).IsRequired(true);
			builder.Property(x => x.DeletedDate).IsRequired(false);
			builder.Property(x => x.ModifiedDate).IsRequired(false);
			builder.Property(x => x.Status).IsRequired(true);
		}
	}
}
