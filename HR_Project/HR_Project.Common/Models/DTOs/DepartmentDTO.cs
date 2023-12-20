using HR_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //Navigation
        public Guid ManagerId { get; set; }
        public int CompanyId { get; set; }
        public ICollection<Personnel> Personnels { get; set; }
    }
}
