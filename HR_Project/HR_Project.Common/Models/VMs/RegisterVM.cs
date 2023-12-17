using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.VMs
{
	public class RegisterVM
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Title { get; set; }
		public string PhoneNumber { get; set; }
		public string City { get; set; }
		public Region Region { get; set; }
		public BloodType BloodType { get; set; }
		public DateTime BirthDate { get; set; }
	}
}
