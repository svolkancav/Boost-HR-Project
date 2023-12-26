using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Enum;

namespace HR_Project.Domain.Entities.Concrete
{
    public class MasterExpense : BaseEntity
    {
        public MasterExpense()
        {
            Expenses = new HashSet<Expense>();
        }

        public int Id { get; set; }
        public double AggregateAmount { get; set; }
		public ConditionType Condition { get; set; }

		//Navigation
		public Guid PersonnelId { get; set; }
		public Personnel Personnel { get; set; }
		public ICollection<Expense> Expenses { get; set; }
    }
}
