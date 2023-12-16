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
    public class MasterExpenseConfig : BaseEntityConfig<MasterExpense> , IEntityTypeConfiguration<MasterExpense>
    {
        public override void Configure(EntityTypeBuilder<MasterExpense> builder)
        {
            builder.HasMany(x => x.Expenses).WithOne(x => x.MasterExpense).HasForeignKey(x => x.MasterExpenseId);

            base.Configure(builder);
        }
    }
}
