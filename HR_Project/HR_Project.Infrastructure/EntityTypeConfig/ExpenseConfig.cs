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
            builder.Property(e => e.Explanation).IsRequired(true);
            builder.Property(e => e.Condition).IsRequired(true);
            builder.Property(e => e.PersonnelId).IsRequired(true);

            builder.HasOne(e=>e.Personnel).WithMany(e=>e.Expenses).HasForeignKey(e=>e.PersonnelId);

            base.Configure(builder);

        }
    }
}
