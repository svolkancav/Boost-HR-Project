using HR_Project.Domain.Entities.Concrete;
using HR_Project.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Enum;

namespace HR_Project.Application.SeedData
{
    public static class SeedData
    {
        public static async void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                AppDbContext context = scope.ServiceProvider.GetService<AppDbContext>();
                context.Database.Migrate();

                var titles = new[] { "Yazılımcı", "Muhasebeci", "Satın Alma", "Pazarlama", "IT", "IK" };

                if (!context.Personeler.Any())
                {
                    var personnellFaker = new Faker<Personnel>()
                        .RuleFor(i => i.Name, i => i.Person.FirstName)
                        .RuleFor(i => i.Surname, i => i.Person.LastName)
                        .RuleFor(i => i.Email, i => i.Person.Email)
                        .RuleFor(i => i.City, i => i.Person.Address.City)
                        .RuleFor(i => i.Address, i => i.Address.FullAddress())
                        .RuleFor(i => i.BirthDate, i => i.Person.DateOfBirth)
                        .RuleFor(i => i.BloodType, i => i.PickRandom<BloodType>())
                        .RuleFor(i => i.PasswordHash, i => i.Internet.Password())
                        .RuleFor(i => i.Title, i => i.PickRandom(titles));

                    var genereatedPersonnels = personnellFaker.Generate(10);

                    context.Personeler.AddRange(genereatedPersonnels);

                    context.SaveChangesAsync();

                }

                if (!context.Advances.Any())
                {
                    var advancesFaker = new Faker<Advance>()
                        .RuleFor(i => i.PersonnelId, i => i.PickRandom<Personnel>().Id)
                        .RuleFor(i => i.Amount, i => i.Random.Int(20, 500))
                        .RuleFor(i => i.Reason, i => i.Lorem.Sentence(1))
                        .RuleFor(i => i.Condition, i => i.PickRandom<ConditionType>())
                        .RuleFor(i => i.LastPaidDate, i => i.Date.Recent(100));

                    var genereatedAdvances = advancesFaker.Generate(10);

                    context.Advances.AddRange(genereatedAdvances);

                    context.SaveChangesAsync();
                }

                if (context.Absences.Any())
                {
                    var absencesFaker = new Faker<Absence>()
                       .RuleFor(i => i.PersonnelId, i => i.PickRandom<Personnel>().Id)
                       .RuleFor(i => i.Reason, i => i.Lorem.Sentence(1))
                       .RuleFor(i => i.Condition, i => i.PickRandom<ConditionType>())
                       .RuleFor(i => i.StartDate, i => i.Date.Recent(100))
                       .RuleFor(i => i.EndDate, i => i.Date.Recent(10))
                       .RuleFor(i => i.LeaveTypes, i => i.PickRandom<LeaveTypes>());

                    var genereatedAbsences = absencesFaker.Generate(10);

                    context.Absences.AddRange(genereatedAbsences);

                    context.SaveChangesAsync();
                }
            }
        }
    }
}
