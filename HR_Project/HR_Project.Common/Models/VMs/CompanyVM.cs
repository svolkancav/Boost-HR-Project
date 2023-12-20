using HR_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.VMs
{
    public class CompanyVM
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public int PersonnelCount { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string? Address { get; set; }
        //Navigation
        public int? CityId { get; set; }
        public City? City { get; set; }
        public int? RegionId { get; set; }
        public Region Region { get; set; }
    }
}
