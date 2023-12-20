using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.VMs
{
	public class PersonelVM
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string? Address { get; set; }
		public string Title { get; set; }
        public string Department { get; set; }
    }
}
