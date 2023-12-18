
﻿using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Common.Models.DTOs
{
	public class RegisterDTO
	{

		[Display(Name = "Kullanıcı Adı")]
		[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
		public string UserName { get; set; }


		[Display(Name = "Email")]
		[Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
		public string Email { get; set; }


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


		[Display(Name = "Şehir")]
		[Required(ErrorMessage = "Şehir alanı boş bırakılamaz.")]
		public string City { get; set; }


		[Display(Name = "İlçe")]
		[Required(ErrorMessage = "İlçe alanı boş bırakılamaz.")]
		public Region Region { get; set; }


		[Display(Name = "Kan Grubu")]
		[Required(ErrorMessage = "Kan Grubu alanı boş bırakılamaz.")]
		public BloodType BloodType { get; set; }


		[Display(Name = "Doğum Günü")]
		[Required(ErrorMessage = "Doğum Günü alanı boş bırakılamaz.")]
		public DateTime BirthDate { get; set; }
		public Gender Gender { get; set; }
		public Nation Nation { get; set; }
		public AccountStatus AccountStatus { get; set; }
	}

}

