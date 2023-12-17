using HR_Project.Common.Models.DTOs;
using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Project.Common.Models.VMs
{
    public class Expense_MasterExpenseVM
    {
        public Expense_MasterExpenseVM() 
        {
            Expense = new ExpenseDTO();
            Expenses = new List<ExpenseDTO>();

        }

		public int Id { get; set; }
		public List<ExpenseDTO> Expenses { get; set; }
        public ExpenseDTO Expense { get; set; }

        //public double AggregateAmount { get; set; }

        public ExpenseType ExpenseType { get; set; }

        public DateTime ExpenseDate { get; set; }

        public double ExpenseAmount { get; set; }

        public Currency Currency { get; set; }

        public string Reason { get; set; }

        public ConditionType Condition { get; set; }

    }
}
