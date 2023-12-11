
using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly SignInManager<Personnel> _signInManager;
        private readonly UserManager<Personnel> _userManager;

        public PersonnelService(IPersonelRepository personelRepository, IMapper mapper, SignInManager<Personnel> signInManager, UserManager<Personnel> userManager)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(RegisterDTO model)
        {
            //TODO auto mapper
            Personnel user = new Personnel
            {
                UserName = model.Name,
                Name = model.Name,
                Email = model.Email,
                CreatedDate = DateTime.Now,
                Title = model.Title,
                Surname = model.Surname,
                CompanyId = model.CompanyId,

            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);
            return result;
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

		public async Task<SignInResult> Login(LoginDTO model)
		{
            var users = await _userManager.Users.ToListAsync();
            var user = await _userManager.FindByEmailAsync(model.Email);
            //PasswordHasher<Personnel> hasher = new PasswordHasher<Personnel>();
            //var password = hasher.HashPassword(user, model.Password);
            return await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
        }
	}
}
