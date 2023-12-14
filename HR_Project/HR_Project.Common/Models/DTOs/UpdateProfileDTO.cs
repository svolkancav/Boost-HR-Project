using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Extensions;
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
		public string? City { get; set; }
		public string? Region { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public BloodType? BloodType { get; set; }
        public string? ImagePath { get; set; }
        //[PictureFileExtension]
        //public IFormFile? UploadPath { get; set; }
        //public int? DepartmentId { get; set; }
        //public Department? Department { get; set; }
    }
}
