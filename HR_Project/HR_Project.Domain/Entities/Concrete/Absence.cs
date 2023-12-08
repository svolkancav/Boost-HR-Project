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

    public class Absence : IBaseEntity

	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public TimeSpan AbsenceDuration { get; set; }
		public LeaveTypes LeaveTypes { get; set; }
		public string Reason { get; set; }

        //IBaseEntity
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        //Navigation
        public int PersonelId { get; set; }
		public Personel Personel { get; set; }

        
    }
}
