using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Entities.Concrete
{
	public class Absence
	{
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public TimeSpan AbsenceDuration { get; set; }
        //public AbsenceType AbsenceType { get; set; }
        public string Reason { get; set; }

        public int PersonnelId { get; set; }
		public Personnel Personnel { get; set; }
	}
}
