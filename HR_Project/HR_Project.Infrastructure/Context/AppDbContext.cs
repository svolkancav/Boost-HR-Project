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
    public class AppDbContext : IdentityDbContext<Personel, IdentityRole<int>, int>
    {
        //ToDo : Eklenecek
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<Personel> Personeler { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Personel>()
                .HasOne(x => x.Department)
                .WithMany(x => x.Personeller)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Absence>()
                .HasOne(x => x.Personel)
                .WithMany(x => x.Absences)
                .HasForeignKey(x => x.PersonelId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Advance>()
                .HasOne(x => x.Personel)
                .WithMany(x => x.Advances)
                .HasForeignKey(x => x.PersonelId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }

    }
}
