using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.Models.DTOs
{
	public class PersonnelsListDTO
	{
		public Guid Id { get; set; }
		public ICollection<AbsenceVM>? Absences { get; set; }
		public ICollection<AdvanceVM>? Advances { get; set; }
        public ICollection<MasterExpenseVM> MasterExpenses { get; set; }
    }
}
