using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using HR_Project.Application.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HR_Project.Application.Services.PersonelServices
{
	public class PersonnelService : IPersonnelService
	{
		private readonly IPersonelRepository _personelRepository;
		private readonly IMapper _mapper;
		private readonly SignInManager<Personnel> _signInManager;
		private readonly UserManager<Personnel> _userManager;
		private readonly IProfileImageService _profileImageService;
		private readonly IConfiguration _configuration;


		public PersonnelService(IPersonelRepository personelRepository, IMapper mapper, SignInManager<Personnel> signInManager, UserManager<Personnel> userManager, IProfileImageService profileImageService, IConfiguration configuration)
		{
			_personelRepository = personelRepository;
			_mapper = mapper;
			_signInManager = signInManager;
			_userManager = userManager;
			_profileImageService = profileImageService;
			_configuration = configuration;
		}

		public async Task<IdentityResult> Register(RegisterDTO model)
		{
			//TODO auto mapper
			Personnel user = new Personnel
			{
				Email = model.Email,
				PasswordHash = model.Password,
				Name = model.Name,
				Surname = model.Surname,
				Title = model.Title,
				PhoneNumber = model.PhoneNumber,
				Address = model.Address,
				RegionId = model.RegionId,
				CityId = model.CityId,
				UserName = model.Name,
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

		public async Task Create(UpdateProfileDTO model)
		{
			Personnel personnel = new Personnel
			{
				Id = model.Id,
				Email = model.Email,
				PasswordHash = model.Password,
				Name = model.Name,
				Surname = model.Surname,
				Title = model.Title,
				PhoneNumber = model.PhoneNumber,
				Gender = model.Gender,
				Nation = model.Nation,
				CityId = model.CityId,
				RegionId = model.RegionId,
				BirthDate = model.BirthDate,
				BloodType = model.BloodType,
				HireDate = model.HireDate,
				ManagerId = model.ManagerId,
				DepartmentId = model.DepartmentId,
				CompanyId = model.CompanyId,
				Address = model.Address,


			};

			await _personelRepository.Create(personnel);
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
					HireDate = x.HireDate,
					PhoneNumber = x.PhoneNumber,
					ManagerId = x.ManagerId,
					CompanyId = x.CompanyId,
					DepartmentId = x.DepartmentId,
					RegionId = x.RegionId,
					CityId = x.CityId,
					Nation = x.Nation,
					Gender = x.Gender,

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
				personel.HireDate = model.HireDate;



				//personel.ImagePath = model.ImagePath;
				await _profileImageService.UploadFile(personel.Id.ToString(), model.UploadImage);

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
			try
			{
				var person = await _personelRepository.GetDefault(x => x.Id == Guid.Parse(id));
				if (person.ManagerId == null)
				{

					var personnel = await _personelRepository.GetFilteredFirstOrDefault(
					x => new UpdateProfileDTO { BirthDate = x.BirthDate, ManagerId = x.ManagerId,ManagerName="Yönetici", Name = x.Name, Surname = x.Surname, Nation = x.Nation, Gender = x.Gender, CompanyId = x.CompanyId, CompanyName = x.Company.Name, DepartmentId = x.DepartmentId, DepartmentName = x.Department.Name, BloodType = x.BloodType, PhoneNumber = x.PhoneNumber, RegionId = x.RegionId, CityId = x.CityId, Title = x.Title, RegionName = x.Region.Name, CityName = x.City.Name, Address = x.Address, Email = x.Email, HireDate = x.HireDate, ImagePath = $"{_configuration["BaseStorageUrl"]}/{x.PersonnelPicture.FilePath}", Id = x.Id },
					where: x => x.Id == Guid.Parse(id),
					include: x => x.Include(d => d.Department).Include(c => c.Company).Include(x => x.Region).Include(x => x.City).Include(x => x.PersonnelPicture)
					);
					return personnel;
				}
				else
				{
					var personnel = await _personelRepository.GetFilteredFirstOrDefault(
									x => new UpdateProfileDTO { BirthDate = x.BirthDate, ManagerId = x.ManagerId, ManagerName = $"{x.Manager.Name} {x.Manager.Surname}", Name = x.Name, Surname = x.Surname, Nation = x.Nation, Gender = x.Gender, CompanyId = x.CompanyId, CompanyName = x.Company.Name, DepartmentId = x.DepartmentId, DepartmentName = x.Department.Name, BloodType = x.BloodType, PhoneNumber = x.PhoneNumber, RegionId = x.RegionId, CityId = x.CityId, Title = x.Title, RegionName = x.Region.Name, CityName = x.City.Name, Address = x.Address, Email = x.Email, HireDate = x.HireDate, ImagePath = $"{_configuration["BaseStorageUrl"]}/{x.PersonnelPicture.FilePath}", Id = x.Id },
									where: x => x.Id == Guid.Parse(id),
									include: x => x.Include(d => d.Department).Include(x => x.Manager).Include(c => c.Company).Include(x => x.Region).Include(x => x.City).Include(x => x.PersonnelPicture)
									);
					return personnel;
				}

			}
			catch (Exception message)
			{

				throw message;
			}

		}
	}
}
