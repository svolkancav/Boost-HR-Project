using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
    public class CompanyRegisterDTO
    {
        public int Id { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        public string Email { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola alanı boş bırakılamaz.")]
        public string Password { get; set; }


        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Adı alanı boş bırakılamaz.")]
        public string Name { get; set; }

        [Display(Name = "Şirket Adı")]
        [Required(ErrorMessage = "Şirket Adı alanı boş bırakılamaz.")]
        public string CompanyName { get; set; }


        [Display(Name = "Soyadı")]
        [Required(ErrorMessage = "Soyadı alanı boş bırakılamaz.")]
        public string Surname { get; set; }


        [Display(Name = "Ünvan")]
        [Required(ErrorMessage = "Ünvan alanı boş bırakılamaz.")]
        public string Title { get; set; }


        public List<SelectListItem>? Regions { get; set; }
        public List<SelectListItem>? CityList { get; set; }

        public int? RegionId { get; set; }
        public int? CityId { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Required(ErrorMessage = "Telefon numarası alanı boş bırakılamaz.")]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Guid? ManagerId { get; set; }
        [Display(Name = "Personel Sayısı")]
        [Required(ErrorMessage = "Personel Sayısı boş bırakılamaz")]
        public int? PersonnelCount { get; set; }

    }
}
