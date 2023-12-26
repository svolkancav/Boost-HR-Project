using HR_Project.Common.Models.Abstract;
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

namespace HR_Project.Common.Models.DTOs
{
	public class MasterExpenseDTO : IMasterExpense
	{
		public MasterExpenseDTO()
		{
			Expenses = new List<ExpenseDTO>();
		}
		public ConditionType Condition { get; set; }
		public int? MasterExpenseId { get; set; }
		public DateTime CreateDate { get; set; }
		public double AggregateAmount { get; set; }
		[ValidateExpenseList]
		public List<ExpenseDTO> Expenses { get; set; }
        public Guid PersonnelId { get; set; }
    }
}
