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
using System.Globalization;
using HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Domain.Entities.Concrete
{

	public class Personnel : IdentityUser<Guid>, IBaseEntity

	{
        public Personnel()
        {
            Personnels = new HashSet<Personnel>();
            Absences = new HashSet<Absence>();
            Advances = new HashSet<Advance>();
			MasterExpenses = new HashSet<MasterExpense>();
            
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Title { get; set; }
        public int? ImageId { get; set; }
        public Gender? Gender { get; set; }
        public Nation? Nation { get; set; }
        public bool IsAccountConfirmed { get; set; }= false;

        public DateTime? BirthDate { get; set; }
		public DateTime? HireDate { get; set; }
        public BloodType? BloodType { get; set; }

        //IBaseEntity
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }= Status.Inserted;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;

        //Navigation

        public int? CityId { get; set; }
        public City City { get; set; }
        public int? RegionId { get; set; }
        public Region Region { get; set; }
        public int? DepartmentId { get; set; }
		public Department Department { get; set; }
		public Guid? ManagerId { get; set; }
		public Personnel Manager { get; set; }
        public int? CompanyId { get; set; }
		public Company Company { get; set; }
		public ICollection<Personnel> Personnels { get; set; }
        public ICollection<Absence> Absences { get; set; }
        public ICollection<Advance> Advances { get; set; }
        public ICollection<MasterExpense> MasterExpenses { get; set; }
        public PersonnelPicture PersonnelPicture { get; set; }

	}
}
