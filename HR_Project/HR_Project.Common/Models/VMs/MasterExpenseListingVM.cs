using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common.Models.VMs
{
    public class MasterExpenseListingVM
    {
        public MasterExpenseListingVM() 
        {
            Expense = new ExpenseDTO();
            Expenses = new List<ExpenseDTO>();

        }

        public List<ExpenseDTO> Expenses { get; set; }
        public ExpenseDTO Expense { get; set; }

    }
}
