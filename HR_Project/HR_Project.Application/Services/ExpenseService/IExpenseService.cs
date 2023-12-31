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
        Task Create(MasterExpenseDTO model);
        Task Update(UpdateMasterExpenseDTO model);

        Task Delete(int id);

        Task<List<MasterExpenseVM>>GetByCondition(ConditionType conditionType);
        Task<List<PersonnelsListDTO>> GetPendingExpense();

        Task<List<MasterExpenseVM>> GetExpenses();

        Task<UpdateMasterExpenseDTO> GetById(string id);



    }
}
