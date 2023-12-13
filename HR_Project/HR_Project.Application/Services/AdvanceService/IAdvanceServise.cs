using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;

namespace HR_Project.Application.Services.AdvanceService
{
	public interface IAdvanceServise
	{
		Task Create(CreateAdvanceDTO model);
		Task Update(UpdateAdvanceDTO model);
		Task Delete(int id);
		Task<List<AdvanceVM>> GetByCondition(ConditionType condition);
		Task<UpdateAdvanceDTO> GetById(string id);
		Task<List<AdvanceVM>> GetAdvances();
	}
}
