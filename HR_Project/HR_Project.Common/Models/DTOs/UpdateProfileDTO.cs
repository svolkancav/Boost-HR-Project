using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Extensions;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateProfileDTO
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
		
        public string? RegionName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public BloodType? BloodType { get; set; }
        public string? ImagePath { get; set; }

        public Gender Gender { get; set; }
        public Nation Nation { get; set; }
        //public string ManagerName { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }
        public Guid? ManagerId { get; set; }
        public Personnel Manager { get; set; }
        public ICollection<Personnel> Personnels { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
        public AccountStatus AccountStatus { get; set; }

        //[PictureFileExtension]
        //public IFormFile? UploadPath { get; set; }
        //public int? DepartmentId { get; set; }
        //public Department? Department { get; set; }
    }
}
