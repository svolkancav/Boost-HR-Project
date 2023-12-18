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

                var titles = new[] { "Mühendis", "Geliştirici", "Teknisyen", "Tekniker", "Baş Mühendis", "Uzman" };
                var department = new[] { "IK", "IT", "Satın alma", "Muhasebe", "Finans", "İdari İşler" };

                if (!context.Companies.Any())
                {
                    var companyFaker = new Faker<Company>()
                        .RuleFor(i => i.Name, i => i.Name.JobArea())
                        //.RuleFor(i => i.Region, i => i.Person.Address.Street)
                        .RuleFor(i => i.City, i => i.Person.Address.City)
                        .RuleFor(i => i.Country, i => i.Person.Address.State)
                        .RuleFor(i => i.PersonnelCount, i => i.Random.Int(1, 50))
                        .RuleFor(i => i.TaxOffice, i => i.Person.Address.Suite)
                        .RuleFor(i => i.TaxNumber, i => i.Random.Int(123456, 456798).ToString())
                        .RuleFor(i => i.Phone, i => i.Random.Int(123456, 456798).ToString());

                    var generatedCompanies = companyFaker.Generate(7);
                    context.Companies.AddRange(generatedCompanies);

                    await context.SaveChangesAsync();
                }


                if (!context.Personnels.Any())
                {
                    List<Company> companies = new List<Company>();
                    companies = await context.Companies.ToListAsync();
                    List<Department> departments = new List<Department>();
                    departments = await context.Departments.ToListAsync();
                    var personnellFaker = new Faker<Personnel>()
                        .RuleFor(i => i.Name, i => i.Person.FirstName)
                        .RuleFor(i => i.Surname, i => i.Person.LastName)
                        .RuleFor(i => i.Email, i => i.Person.Email)
                        .RuleFor(i => i.City, i => i.Person.Address.City)
                        .RuleFor(i => i.Address, i => i.Address.FullAddress())
                        .RuleFor(i => i.BirthDate, i => i.Person.DateOfBirth)
                        .RuleFor(i => i.BloodType, i => i.PickRandom<BloodType>())
                        .RuleFor(i => i.PasswordHash, i => i.Internet.Password())
                        .RuleFor(i => i.Title, i => i.PickRandom(titles))
                        .RuleFor(i => i.PhoneNumber, i => i.Person.Phone)
                        .RuleFor(i => i.CompanyId, i => i.PickRandom(companies).Id)
                        .RuleFor(i => i.Nation, i => i.PickRandom<Nation>())
                        .RuleFor(i => i.Gender, i => i.PickRandom<Gender>());
                        
                    

                    var genereatedPersonnels = personnellFaker.Generate(50);

                    context.Personnels.AddRange(genereatedPersonnels);

                    await context.SaveChangesAsync();

                }
                if (!context.Departments.Any())
                {
                    List<Company> companies = new List<Company>();
                    companies = await context.Companies.ToListAsync();
                    List<Personnel> personnels = new List<Personnel>();
                    personnels = await context.Personnels.ToListAsync();

                    var departmentFaker = new Faker<Department>()
                        .RuleFor(i => i.Name, i => i.PickRandom(department))
                        .RuleFor(i => i.CompanyId, i => i.PickRandom(companies).Id)
                        .RuleFor(i => i.Description, i => i.Lorem.Sentence(2))
                        .RuleFor(i => i.ManagerId, i => i.PickRandom(personnels).Id);

                    var generatedDepartment = departmentFaker.Generate(7);
                    context.Departments.AddRange(generatedDepartment);

                    await context.SaveChangesAsync();

                }

                if (!context.Advances.Any())
                {
                    List<Personnel> personnels= new List<Personnel>();
                    personnels = await context.Personnels.ToListAsync();
                    var advancesFaker = new Faker<Advance>()
                        .RuleFor(i => i.PersonnelId, i => i.PickRandom(personnels).Id)
                        .RuleFor(i => i.Amount, i => i.Random.Int(20, 500))
                        .RuleFor(i => i.Reason, i => i.Lorem.Sentence(1))
                        .RuleFor(i => i.Condition, i => i.PickRandom<ConditionType>())
                        .RuleFor(i => i.LastPaidDate, i => i.Date.Recent(100));

                    var genereatedAdvances = advancesFaker.Generate(50);

                    context.Advances.AddRange(genereatedAdvances);

                    await context.SaveChangesAsync();
                }

                if (!context.Absences.Any())
                {
                    List<Personnel> personnels = new List<Personnel>();
                    personnels = await context.Personnels.ToListAsync();
                    var absencesFaker = new Faker<Absence>()
                       .RuleFor(i => i.PersonnelId, i => i.PickRandom(personnels).Id)
                       .RuleFor(i => i.Reason, i => i.Lorem.Sentence(1))
                       .RuleFor(i => i.Condition, i => i.PickRandom<ConditionType>())
                       .RuleFor(i => i.StartDate, i => i.Date.Recent(100))
                       .RuleFor(i => i.EndDate, i => i.Date.Recent(10))
                       .RuleFor(i => i.LeaveTypes, i => i.PickRandom<LeaveTypes>());

                    var genereatedAbsences = absencesFaker.Generate(50);

                    context.Absences.AddRange(genereatedAbsences);

                    await context.SaveChangesAsync();
                }

                try
                {
                    string illerJSON = System.IO.File.ReadAllText(@"SeedData/iller.json");
                    List<City> cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(illerJSON);
                    context.Cities.AddRange(cities);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Handle the exception (log, rethrow, or perform other actions)
                    Console.WriteLine($"An error occurred while seeding cities from JSON: {ex.Message}");
                }




            }
        }
    }
}
