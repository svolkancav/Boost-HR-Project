﻿using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using HR_Project.Application.Services.EmailService;
using System.Security.Policy;

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
                UserName = model.Email,
				Email = model.Email,
				Name = model.Name,
                CreatedDate = DateTime.Now,
                Title = model.Title,
                PhoneNumber = model.PhoneNumber,
                Surname = model.Surname,
				Gender = model.Gender,
				Nation = model.Nation,
				AccountStatus = AccountStatus.Inactive

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
            Personnel personnel = await _personelRepository.GetDefault(x => x.Id == Guid.Parse(id));
            var model = _mapper.Map<PersonelDTO>(personnel);

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
                    PhoneNumber = x.PhoneNumber,
                    ManagerId = x.ManagerId,
                    Region = x.Region,
                    Nation = x.Nation,
                    Gender = x.Gender,
                    Company = x.Company,


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

		public async Task Update(UpdateProfileDTO model)
        {
            var personel = await _personelRepository.GetDefault(x=>x.Id == model.Id);
            //bool isExist = await _personelRepository.Any(x => x.Id == model.Id);

           

            if (personel is not null)
            {
                
                personel.Name = model.Name;
                personel.PhoneNumber = model.PhoneNumber;
                //personel.Region.Name = model.RegionName; //TODO: kontrol edilecek.
                personel.City = model.City;
                personel.Email = model.Email;
                personel.Address = model.Address;
                personel.BloodType = model.BloodType;
                personel.Surname = model.Surname;
                personel.City = model.City;
                personel.Nation = model.Nation;
                personel.Gender = model.Gender;
                personel.Department = model.Department;
                personel.Manager = model.Manager;
                personel.Company = model.Company;
                personel.Region = model.Region;

                //personel.ImagePath = model.ImagePath;


                await _personelRepository.Update(personel);
            }
        }

		public async Task<SignInResult> Login(LoginDTO model)
		{
            //PasswordHasher<Personnel> hasher = new PasswordHasher<Personnel>();
            //var password = hasher.HashPassword(user, model.Password);
            try
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                return result;
            }
            catch (Exception message)
            {

                throw message;
            }
            
            
        }
	}
}
