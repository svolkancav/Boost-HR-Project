using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.Abstract;
using HR_Project.Common.ValidationClass;
using HR_Project.Domain.Enum;

namespace HR_Project.Common.Models.DTOs
{
	public class UpdateMasterExpenseDTO : IMasterExpense
	{
        public UpdateMasterExpenseDTO()
        {
            Expenses = new List<ExpenseDTO>();
        }
        public int Id { get; set; }

		public DateTime CreateDate { get; set; }

		public double AggregateAmount { get; set; }

		private Currency _currency;

		public Currency Currency
		{
			get { return _currency; }
			set { _currency = Expenses[0].Currency; }
		}

		[ValidateExpenseList]
		public List<ExpenseDTO> Expenses { get; set; }

	}
}
