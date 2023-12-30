using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;

namespace HR_Project.Domain.Entities.Concrete
{

	public class Company : BaseEntity, IEntity<int>
	{
        public Company()
        {
            Departments = new HashSet<Department>();
			Personnels = new HashSet<Personnel>();
        }
        public int Id { get; set; }
		public string Name { get; set; }
		
		public string? Phone { get; set; }
        public int PersonnelCount { get; set; }
        public string? TaxOffice { get; set; }
        public string? TaxNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }

        //Navigation
        public int? RegionId { get; set; }
        public Region Region { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public Status Status { get; set; } = Status.Inserted;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<Department> Departments { get; set; }
        public ICollection<Personnel> Personnels { get; set; }
	}
}
