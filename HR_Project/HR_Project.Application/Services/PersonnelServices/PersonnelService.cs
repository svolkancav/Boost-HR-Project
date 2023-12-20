using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using HR_Project.Application.Services.FileService;
using System.Web.Helpers;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using Microsoft.EntityFrameworkCore;

namespace HR_Project.Application.Services.PersonelServices
{
    public class PersonnelService : IPersonnelService
    {
        private readonly IPersonelRepository _personelRepository;
        private readonly IMapper _mapper;
        private readonly SignInManager<Personnel> _signInManager;
        private readonly UserManager<Personnel> _userManager;
        private readonly IProfileImageService _profileImageService;


        public PersonnelService(IPersonelRepository personelRepository, IMapper mapper, SignInManager<Personnel> signInManager, UserManager<Personnel> userManager, IProfileImageService profileImageService)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _profileImageService = profileImageService;
        }

        public async Task<IdentityResult> Register(RegisterDTO model)
        {
            //TODO auto mapper
            Personnel user = new Personnel
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.Password,
                Name = model.Name,
                Surname = model.Surname,
                Title = model.Title,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                Nation = model.Nation,
                AccountStatus = AccountStatus.Inactive,
                CityId = model.CityId,
                RegionId = model.RegionId,
                BirthDate = model.BirthDate,
                BloodType = model.BloodType,

            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);


                try
                {
                    await _profileImageService.UploadFile(user.Id.ToString(), model.UploadImage);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }



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

        public async Task<PersonelDTO> GetByEmail(string email)
        {
            PersonelDTO personel = _mapper.Map<PersonelDTO>(await _personelRepository.GetDefault(x => x.Email == email));

            personel.ProfilePicture = await _profileImageService.GetFileById(personel.Id.ToString());

            return personel;
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
            var personel = await _personelRepository.GetDefault(x => x.Id == model.Id);
            //bool isExist = await _personelRepository.Any(x => x.Id == model.Id);



            if (personel is not null)
            {

                personel.Name = model.Name;
                personel.PhoneNumber = model.PhoneNumber;
                //personel.Region.Name = model.RegionName; //TODO: kontrol edilecek.
                personel.CityId = model.CityId;
                personel.Email = model.Email;
                personel.Address = model.Address;
                personel.BloodType = model.BloodType;
                personel.Surname = model.Surname;
                personel.Nation = model.Nation;
                personel.Gender = model.Gender;
                personel.DepartmentId = model.DepartmentId;
                personel.ManagerId = model.ManagerId;
                personel.CompanyId = model.CompanyId;
                personel.RegionId = model.RegionId;

                //personel.ImagePath = model.ImagePath;
                _profileImageService.UploadFile(personel.Id.ToString(), model.UploadImage);

                await _personelRepository.Update(personel);
            }
        }

        public async Task<SignInResult> Login(LoginDTO model)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

                SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                return result;


            }
            catch (Exception message)
            {

                throw message;
            }


        }
        public async Task<UpdateProfileDTO> FillDTO(string id)
        {
           var personnel =  await _personelRepository.GetFilteredFirstOrDefault(
                x => new UpdateProfileDTO { BirthDate = x.BirthDate, ManagerId = x.Manager.Id, Name = x.Name, Surname = x.Surname, Nation = x.Nation, Gender = x.Gender, CompanyId = x.Company.Id, DepartmentId = x.Department.Id, DepartmentName = x.Department.Name, CompanyName = x.Company.Name, ManagerName = x.Manager.Name, BloodType = x.BloodType, PhoneNumber = x.PhoneNumber, RegionId = x.Region.Id, CityId = x.CityId, Title = x.Title, RegionName = x.Region.Name, CityName = x.City.Name, Address = x.Address, Id = x.Id, Email = x.Email, HireDate=x.HireDate, ImagePath = x.PersonnelPicture.FilePath},
                where: x => x.Id == Guid.Parse(id),
                include: x => x.Include(d => d.Department).Include(c => c.Company).Include(x => x.Personnels).Include(x => x.Region).Include(x => x.City).Include(x=>x.PersonnelPicture)


                );

            return personnel;
        }
    }
}
