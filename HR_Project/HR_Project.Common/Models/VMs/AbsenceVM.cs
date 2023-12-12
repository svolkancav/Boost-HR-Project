using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.VMs
{
    public class AbsenceVM
    {
        public string Reason { get; set; }
        public LeaveTypes LeaveTypes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AbsenceDuration { get; set; }
        public ConditionType Condition { get; set; }
    }
}
