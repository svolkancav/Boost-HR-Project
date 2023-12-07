using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;

namespace HR_Project.Domain.Entities.Concrete
{
	public class BaseEntity : IBaseEntity
	{
		public DateTime CreatedDate { get; set; }= DateTime.Now;
		public DateTime? ModifiedDate { get; set; }
		public DateTime? DeletedDate  { get; set; }
		public Status Status { get; set; }= Status.Inserted;
	}
}
