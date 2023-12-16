using HR_Project.Common.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.CompanyService
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyService _companyService;

		public CompanyService(ICompanyService companyService)
		{
			_companyService = companyService;
		}

		public Task Create(CreateCompanyDTO model)
		{
			throw new NotImplementedException();
		}

		public Task Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Task<UpdateCompanyDTO> GetById(string id)
		{
			throw new NotImplementedException();
		}

		public Task Update(UpdateCompanyDTO model)
		{
			throw new NotImplementedException();
		}
	}
}
