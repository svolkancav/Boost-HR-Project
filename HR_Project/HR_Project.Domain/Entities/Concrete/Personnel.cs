using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Personnel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? HireDate { get; set; }
		//Todo: Add Bloodtype enum
        //public Bloodtype? Bloodtype { get; set; }

        public int? DepartmentId { get; set; }
		public Department Department { get; set; }

		public int? ManagerId { get; set; }
		public Personnel Manager { get; set; }

		public ICollection<Personnel> Subordinates { get; set; }
		//public ICollection<Salary> Salaries { get; set; }
		public ICollection<Absence> Absences { get; set; }
		public ICollection<Advance> Advance { get; set; }

	}
}
