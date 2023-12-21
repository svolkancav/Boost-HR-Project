using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HR_Project.Common.Extensions;
using HR_Project.Common.Models.VMs;
using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateProfileDTO
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
		
        public string? RegionName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public BloodType? BloodType { get; set; }
        public int? ImageId { get; set; }
		[PictureFileExtension]
		public IFormFile? UploadImage { get; set; }
        public string? ImagePath { get; set; }

        public Gender Gender { get; set; }
        public Nation Nation { get; set; }
        //public string ManagerName { get; set; }

        public int? CityId { get; set; }
        public Guid? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public ICollection<Personnel>? Personnels { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public int? RegionId { get; set; }
        public string? CityName { get; set; }

        public List<SelectListItem>? Regions { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        public List<SelectListItem>? Managers { get; set; }
        public List<SelectListItem>? CityList { get; set; }

        //[PictureFileExtension]
        //public IFormFile? UploadPath { get; set; }
        //public int? DepartmentId { get; set; }
        //public Department? Department { get; set; }
    }
}
