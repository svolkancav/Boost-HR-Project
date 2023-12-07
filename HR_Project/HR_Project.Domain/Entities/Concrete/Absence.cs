using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Leave:BaseEntity, IEntity<int>
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public TimeSpan AbsenceDuration { get; set; }
		public LeaveTypes LeaveTypes { get; set; }
		public string Reason { get; set; }

        public int PersonnelId { get; set; }
		public Personnel Personnel { get; set; }
	}
}
