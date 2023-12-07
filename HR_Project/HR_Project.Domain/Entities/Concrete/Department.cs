using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Department: BaseEntity, IEntity<int>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public int? ManagerId { get; set; }

        public Personnel Manager { get; set; }
        public ICollection<Personnel> Personnel { get; set; }
	}
}
