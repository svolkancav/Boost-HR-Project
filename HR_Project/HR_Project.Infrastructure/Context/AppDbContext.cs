using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using HR_Project.Infrastructure.EntityTypeConfig;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File=HR_Project.Domain.Entities.Concrete.FileEntities.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<Personnel, IdentityRole<Guid>, Guid>
    {
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<PersonnelPicture> PersonnelPictures { get; set; }
        public DbSet<CostPicture> CostPictures { get; set; }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<MasterExpense> MasterExpenses { get; set; }



        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PersonnelConfig());
            builder.ApplyConfiguration(new AdvanceConfig());
            builder.ApplyConfiguration(new AbsenceConfig());
            builder.ApplyConfiguration(new CompanyConfig());
            builder.ApplyConfiguration(new DepartmentConfig());
            builder.ApplyConfiguration(new ExpenseConfig());
            builder.ApplyConfiguration(new MasterExpenseConfig());
            builder.ApplyConfiguration(new FileConfig());


            base.OnModelCreating(builder);
        }

    }
}
