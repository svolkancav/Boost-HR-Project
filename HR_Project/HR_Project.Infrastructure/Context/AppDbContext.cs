using HR_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<Personnel, IdentityRole<Guid>, Guid>
    {
        //ToDo : Eklenecek
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Personnel> Personeler { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Personnel>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Personnels)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Absence>()
                .HasOne(x => x.Personnel)
                .WithMany(x => x.Absences)
                .HasForeignKey(x => x.PersonnelId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Advance>()
                .HasOne(x => x.Personnel)
                .WithMany(x => x.Advances)
                .HasForeignKey(x => x.PersonnelId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }

    }
}
