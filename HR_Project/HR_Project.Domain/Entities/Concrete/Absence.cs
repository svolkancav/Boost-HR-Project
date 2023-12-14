using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Enum;

namespace HR_Project.Domain.Entities.Concrete
{

    public class Absence : BaseEntity, IEntity<int>

	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public long AbsenceDuration { get; set; }
		public LeaveTypes LeaveTypes { get; set; }
		public string Reason { get; set; }
		public ConditionType Condition { get; set; }

		//Navigation
		public Guid PersonnelId { get; set; }
		public Personnel Personnel { get; set; }

        
    }
}
