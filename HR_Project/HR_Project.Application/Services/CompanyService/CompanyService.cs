using AutoMapper;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HR_Project.Application.Services.CompanyService
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepository _companyRepository;
		private readonly IMapper _mapper;

		public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
		{
			_companyRepository = companyRepository;
			_mapper = mapper;
		}

		public Task Create(CompanyRegisterDTO model)
		{
			try
			{
				Company company = new Company();
				company.Address = model.Address;
				company.Phone = model.PhoneNumber;
				company.CityId = model.CityId;
				company.Name = model.CompanyName;
				company.RegionId = model.RegionId;
				company.PersonnelCount = model.PersonnelCount;

				return _companyRepository.Create(company);
            }
			catch (Exception m)
			{

				throw m;
			}
			

			

		}

		public async Task Delete(int id)
		{
			Company company =await _companyRepository.GetDefault(x => x.Id == id);

			if (id == 0)
			{
				throw new ArgumentException("Id 0 Olamaz!");

			}
			else if (company == null)
			{
				throw new ArgumentException("Böyle bir şirket mevcut değil!");
			}

			company.DeletedDate = DateTime.Now;
			company.Status = Status.Deleted;
			await _companyRepository.Delete(company);

		}

		public async Task<UpdateCompanyDTO> GetById(string id)
		{
			Company company =await _companyRepository.GetDefault(x => x.Id == Convert.ToInt32(id));

			return _mapper.Map<UpdateCompanyDTO>(company);
		}

        public async Task<List<UpdateCompanyDTO>> GetCompanies()
        {
            return await _companyRepository.GetFilteredList(x => new UpdateCompanyDTO
            {
				Id=x.Id,
				Name = x.Name,
                Phone = x.Phone,
                PersonnelCount = x.PersonnelCount,
                TaxOffice = x.TaxOffice,
                TaxNumber = x.TaxNumber,
                Address = x.Address,
                CityId = x.CityId,
                RegionId = x.RegionId,
				Email = x.Email

            }, x => x.Status != Status.Deleted);
        }

        public async Task Update(UpdateCompanyDTO model)
		{
			Company company=await _companyRepository.GetDefault(x => x.Id == model.Id);

			company.Phone=model.Phone;
			company.Name=model.Name;
			company.Address=model.Address;
			company.TaxNumber=model.TaxNumber;
			company.TaxOffice=model.TaxOffice;
			company.PersonnelCount=model.PersonnelCount;
			company.ModifiedDate=DateTime.Now;
			company.Status=Status.Updated;
			company.RegionId=model.RegionId;
			company.CityId = model.CityId;
			company.Email = model.Email;

			await _companyRepository.Update(company);
		}
	}
}
