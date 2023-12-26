using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Common.Models.Abstract
{
	public interface IMasterExpense
	{
		public List<ExpenseDTO> Expenses { get; set; }
	}
}
