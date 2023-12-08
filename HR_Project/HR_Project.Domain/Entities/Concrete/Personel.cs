using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace HR_Project.Domain.Entities.Concrete
{

	public class Personel : IdentityUser<int>, IBaseEntity
	{
        public Personel()
        {
            CreatedDate = DateTime.Now;
            Status = Status.Inserted;
        }

        public int Id { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
		public string Surname { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string Title { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }

		public DateTime? BirthDate { get; set; }
		public DateTime? HireDate { get; set; }

		public BloodType? BloodType { get; set; }

		//IBaseEntity
		public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

		//Navigation

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }


        public ICollection<Advance> Advances { get; set; }
        public ICollection<Absence> Absences { get; set; }





	}
}
