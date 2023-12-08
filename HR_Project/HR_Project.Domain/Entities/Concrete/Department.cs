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

	public class Department : BaseEntity, IEntity<int>

	{
        public Department()
        {
            Personnels = new HashSet<Personnel>();
        }
        public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }


        //Navigation
		public Guid? ManagerId { get; set; }
        public Personnel Manager { get; set; }
        public ICollection<Personnel> Personnels { get; set; }
	}
}
