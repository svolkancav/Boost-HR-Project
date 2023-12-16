using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.DTOs
{
    public class MasterExpenseDTO
    {
        public int MasterExpenseId { get; set; }

        public int ExpenseId { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public DateTime ExpenseDate { get; set; }

        public double ExpenseAmount { get; set; }

        public Currency Currency { get; set; }

        public string Reason { get; set; }

        public ConditionType Condition { get; set; }

        public double AggregateAmount { get; set; }

    }
}
