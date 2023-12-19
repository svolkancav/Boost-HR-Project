using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.IoC.Models.DTOs
{
    public class PersonelDTO
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }   
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public BloodType? Bloodtype { get; set; }
        public Gender Gender { get; set; }
        public Nation Nation { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid? ManagerId { get; set; }
        public ICollection<Personnel> Subordinates { get; set; }
        //public ICollection<Salary> Salaries { get; set; }
        public ICollection<Absence> Absences { get; set; }
        public ICollection<Advance> Advances { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }
    }
}
