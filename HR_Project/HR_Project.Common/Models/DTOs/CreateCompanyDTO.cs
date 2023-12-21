using HR_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace HR_Project.Common.Models.DTOs
{
    public class CreateCompanyDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public int PersonnelCount { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        //Navigation
        public int? CityId { get; set; }
        public int? RegionId { get; set; }
        public List<SelectListItem>? Regions { get; set; }
        public List<SelectListItem>? CityList { get; set; }
        //public ICollection<Department> Departments { get; set; }
        //public ICollection<Personnel> Personnels { get; set; }


        //public string Logo { get; set; }

    }
}
