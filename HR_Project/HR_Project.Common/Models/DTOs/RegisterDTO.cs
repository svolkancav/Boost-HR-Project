
﻿using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using HR_Project.Common.Extensions;
using HR_Project.Common.ValidationClass;

namespace HR_Project.Common.Models.DTOs
{
	public class RegisterDTO
	{


		[Display(Name = "Email")]
		[Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
		public string Email { get; set; }

        public string UserName { get; set; }

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
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }


        //[Display(Name = "İlçe")]
        //[Required(ErrorMessage = "İlçe alanı boş bırakılamaz.")]
        //public List<SelectListItem> Region { get; set; }
        public int? RegionId { get; set; }
        public int? CityId { get; set; }
        //[Display(Name = "İl")]
        //[Required(ErrorMessage = "İl alanı boş bırakılamaz.")]
        //public List<SelectListItem> CityList { get; set; }


        [Display(Name = "Kan Grubu")]
		public BloodType? BloodType { get; set; }


		[Display(Name = "Doğum Günü")]
		[Required(ErrorMessage = "Doğum Günü alanı boş bırakılamaz.")]
		public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public Gender Gender { get; set; }
		public Nation Nation { get; set; }
        public List<SelectListItem>? Regions { get; set; }
        public List<SelectListItem>? CityList { get; set; }
        public AccountStatus AccountStatus { get; set; }
		[PictureFileExtension]
        public IFormFile? UploadImage { get; set; }
    }

}

