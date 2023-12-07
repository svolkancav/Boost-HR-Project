using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.PersonelServices
{
    public class PersonelService : IPersonelService
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly IMapper _mapper;

        public PersonelService(IPersonelRepository personelRepository, IMapper mapper)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
        }

        public async Task Create(PersonelDTO model)
        {
            Personel personel = _mapper.Map<Personel>(model);
            await _personelRepository.Create(personel);
        }

        public async Task Delete(int id)
        {
            Personel personel = await _personelRepository.GetDefault(x => x.Id == id);
            personel.DeletedDate = DateTime.Now;
            personel.Status = Domain.Enum.Status.Deleted;
            await _personelRepository.Delete(personel);
        }


        public async Task<PersonelDTO> GetById(int id)
        {
            Personel genre = await _personelRepository.GetDefault(x => x.Id == id);
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

        public async Task Update(PersonelDTO model)
        {
            bool isExist = await _personelRepository.Any(x => x.Id == model.Id);

            if (isExist)
            {
                var personel = _mapper.Map<Personel>(model);
                await _personelRepository.Update(personel);
            }
        }
    }
}
