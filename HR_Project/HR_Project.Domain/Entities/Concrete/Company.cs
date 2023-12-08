﻿using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;

namespace HR_Project.Domain.Entities.Concrete
{

	public class Company : IBaseEntity

	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? Region { get; set; }
<<<<<<< HEAD:HR_Project/HR_Project.Domain/Entities/Concrete/Company.cs
		public string? PostalCode { get; set; }
		public string? Country { get; set; }
		public string? Phone { get; set; }
		public string? Fax { get; set; }
=======
		public DateTime? BirthDate { get; set; }
		public DateTime? HireDate { get; set; }

		public BloodType? BloodType { get; set; }
>>>>>>> BloodType enum eklendi.:HR_Project/HR_Project.Domain/Entities/Concrete/Personnel.cs

		//IBaseEntity
		public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        //Navigation
        public ICollection<Department> Departments { get; set; }
		public ICollection<Personel> Personeller { get; set; }
	}
}
