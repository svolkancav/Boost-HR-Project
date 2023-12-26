using HR_Project.Common.Models.Abstract;
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
    public class MasterExpenseVM : IMasterExpense
    {
        public MasterExpenseVM() 
        {
            Expenses = new List<ExpenseDTO>();
        }

		public int Id { get; set; }
		public List<ExpenseDTO> Expenses { get; set; }
		public ConditionType Condition { get; set; }
        public DateTime CreateDate { get; set; }
        public double AggregateAmount { get; set; }

		private Currency _currency;

		public Currency Currency
		{
			get { return _currency; }
			set { _currency = Expenses[0].Currency; }
		}

		//public double AggregateAmount { get; set; }
	}
}
