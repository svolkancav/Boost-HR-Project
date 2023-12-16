using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task Create(ExpenseDTO model);
        Task Update(UpdateExpenseDTO model);

        Task Delete(int id);

        Task<List<ExpenseVM>>GetByCondition(ConditionType conditionType);

        Task<List<ExpenseVM>> GetExpenses();

        Task<UpdateExpenseDTO> GetById(string id);



    }
}
