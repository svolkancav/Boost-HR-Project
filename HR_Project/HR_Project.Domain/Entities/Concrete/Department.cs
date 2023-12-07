using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Department : IBaseEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public int? ManagerId { get; set; }

        //IBaseEntity
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        //Navigation

        public Personnel Manager { get; set; }
        public ICollection<Personnel> Personnel { get; set; }
	}
}
