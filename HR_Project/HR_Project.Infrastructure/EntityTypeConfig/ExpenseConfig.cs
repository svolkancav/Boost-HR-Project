using HR_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Infrastructure.EntityTypeConfig
{
    public class ExpenseConfig : BaseEntityConfig<Expense>, IEntityTypeConfiguration<Expense>
    {
        public override void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ExpenseType).IsRequired(true);
            builder.Property(e => e.ExpenseDate).IsRequired(true);
            builder.Property(e => e.ExpenseAmount).IsRequired(true);
            builder.Property(e => e.Currency).IsRequired(true);
            builder.Property(e => e.Reason).IsRequired(true);
            builder.Property(e => e.Condition).IsRequired(true);
            builder.Property(e => e.PersonnelId).IsRequired(true);
			builder.Property(x => x.ImageId).IsRequired(false);


			builder.HasOne(e=>e.Personnel).WithMany(e=>e.Expenses).HasForeignKey(e=>e.PersonnelId);

            builder.HasOne(x => x.CostPicture).WithOne(x => x.Expense).HasForeignKey<Expense>(x => x.ImageId).OnDelete(DeleteBehavior.NoAction);

            base.Configure(builder);

        }
    }
}
