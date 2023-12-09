using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;

namespace HR_Project.Application.Services.AbsenceService
{
	public interface IAbsenceService
    {
		Task Create(AbsenceDTO model);
		Task Update(UpdateAbsenceDTO model);
		Task Delete(int id);
		Task<List<AbsenceVM>> GetByCondition(ConditionType condition);
		Task<List<AbsenceVM>> GetAbsences();
	}
}
