
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using HR_Project.Common.ValidationClass;

namespace HR_Project.Common.Models.DTOs
{
    public class RegisterDTO
	{
        public Guid Id { get; set; }

        [Display(Name = "Email")]
		[Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
		public string Email { get; set; }

        public string? UserName { get; set; }

        [Display(Name = "Parola")]
		[Required(ErrorMessage = "Parola alanı boş bırakılamaz.")]
		public string Password { get; set; }


		[Display(Name = "Adı")]
		[Required(ErrorMessage = "Adı alanı boş bırakılamaz.")]
		public string Name { get; set; }


		[Display(Name = "Soyadı")]
		[Required(ErrorMessage = "Soyadı alanı boş bırakılamaz.")]
		public string Surname { get; set; }


		[Display(Name = "Ünvan")]
		[Required(ErrorMessage = "Ünvan alanı boş bırakılamaz.")]
		public string Title { get; set; }


		[Display(Name = "Telefon Numarası")]
		[Required(ErrorMessage = "Telefon numarası alanı boş bırakılamaz.")]
		public string PhoneNumber { get; set; }
		public string? Address { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public Guid? ManagerId { get; set; }


        //[Display(Name = "İlçe")]
        //[Required(ErrorMessage = "İlçe alanı boş bırakılamaz.")]
        //public List<SelectListItem> Region { get; set; }
        public int? RegionId { get; set; }
        public string? CompanyName { get; set; }
        public int? CityId { get; set; }
        //[Display(Name = "İl")]
        //[Required(ErrorMessage = "İl alanı boş bırakılamaz.")]
        //public List<SelectListItem> CityList { get; set; }

        public int PersonnelCount { get; set; }

        [Display(Name = "Kan Grubu")]
		public BloodType? BloodType { get; set; }
		public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public Gender Gender { get; set; }
		public Nation Nation { get; set; }
        public List<SelectListItem>? Regions { get; set; }
        public List<SelectListItem>? CityList { get; set; }
        public List<SelectListItem>? Managers { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        public int? ImageId { get; set; }
        [PictureFileExtension]
        public IFormFile? UploadImage { get; set; }
        public string? ImagePath { get; set; }

    }

}

