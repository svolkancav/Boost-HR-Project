using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
    public class CityDTO
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public ICollection<Region> Regions { get; set; }
        public Status Status { get; set; } = Status.Inserted;
    }
}
