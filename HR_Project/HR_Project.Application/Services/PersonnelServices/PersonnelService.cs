
using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.PersonelServices
{
	public class PersonnelService : IPersonnelService
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly IMapper _mapper;

        public PersonnelService(IPersonelRepository personelRepository, IMapper mapper)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
        }

        public async Task Create(PersonelDTO model)
        {
            Personnel personel = _mapper.Map<Personnel>(model);
            await _personelRepository.Create(personel);
        }

        public async Task Delete(string id)
        {
            Personnel personel = await _personelRepository.GetDefault(x => x.Id == Guid.Parse(id));
            personel.DeletedDate = DateTime.Now;
            personel.Status = Domain.Enum.Status.Deleted;
            await _personelRepository.Delete(personel);
        }


        public async Task<PersonelDTO> GetById(string id)
        {
            Personnel genre = await _personelRepository.GetDefault(x => x.Id == Guid.Parse(id));
            var model = _mapper.Map<PersonelDTO>(genre);

            return model;
        }



        public async Task<List<PersonelDTO>> GetPersonels()
        {
            var personels = await _personelRepository.GetFilteredList(
                select: x => new PersonelDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Email = x.Email,
                    Address = x.Address,
                    BirthDate = x.BirthDate,
                    Department = x.Department,
                    HireDate = x.HireDate,
                    ManagerId = x.ManagerId

                },
                where: x => x.Status != Domain.Enum.Status.Deleted,
                orderBy: x => x.OrderBy(x => x.Name)

                );
            return personels;
        }

		public Task<string[]> GetRoles(string email)
		{
			throw new NotImplementedException();
		}

		public Task<PersonelDTO> Login(LoginDTO model)
		{
			throw new NotImplementedException();
		}

		public void Logout(string token)
		{
			throw new NotImplementedException();
		}

		public async Task Update(PersonelDTO model)
        {
            bool isExist = await _personelRepository.Any(x => x.Id == model.Id);

            if (isExist)
            {
                var personel = _mapper.Map<Personnel>(model);
                await _personelRepository.Update(personel);
            }
        }

		Task<SignInResult> IPersonnelService.Login(LoginDTO model)
		{
			throw new NotImplementedException();
		}
	}
}
