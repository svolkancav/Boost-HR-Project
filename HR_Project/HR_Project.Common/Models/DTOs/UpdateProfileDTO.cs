using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Extensions;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateProfileDTO
	{
        public Guid Id { get; set; }
        public string? Address { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
		public BloodType? BloodType { get; set; }
        public string? ImagePath { get; set; }
        [PictureFileExtension]
        public IFormFile UploadPath { get; set; }
    }
}
