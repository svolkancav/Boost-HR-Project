using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Department
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public int? ManagerId { get; set; }

        public Personnel Manager { get; set; }
        public ICollection<Personnel> Personnel { get; set; }
	}
}
