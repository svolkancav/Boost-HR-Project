using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.CompanyService
{
	public interface ICompanyService
	{
		Task Create(CreateCompanyDTO model);
		Task Update(UpdateCompanyDTO model);
		Task Delete(int id);
        Task<List<UpdateCompanyDTO>> GetCompanies();
        Task<UpdateCompanyDTO> GetById(string id);
	}
}
