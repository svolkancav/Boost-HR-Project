using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Navigation

        public ICollection<Expense> Expenses { get; set; }
    }
}
