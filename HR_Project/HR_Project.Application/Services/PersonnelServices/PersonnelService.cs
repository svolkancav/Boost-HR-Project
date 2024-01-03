using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using HR_Project.Application.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using Org.BouncyCastle.Math.EC.Rfc7748;

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
		private readonly ICompanyRepository _companyRepository;
		private readonly IDepartmentRepository _departmentRepository;


        public PersonnelService(IPersonelRepository personelRepository, IMapper mapper, SignInManager<Personnel> signInManager, UserManager<Personnel> userManager, IProfileImageService profileImageService, IConfiguration configuration, ICompanyRepository companyRepository, IDepartmentRepository departmentRepository)
        {
            _personelRepository = personelRepository;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _profileImageService = profileImageService;
            _configuration = configuration;
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IdentityResult> Register(RegisterDTO model)
		{
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

		public async Task Create(CreateProfileDTO model)
		{
			Personnel personnel = new Personnel
			{
				Id = model.Id,
				Email = model.Email,
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
			var company = await _companyRepository.GetDefault(x => x.Id == personnel.CompanyId);
			var department = await _departmentRepository.GetDefault(x => x.Id == personnel.DepartmentId);
            var manager = await _personelRepository.GetDefault(x => x.Id == personnel.ManagerId);
			
			
			model.ManagerName = manager is not null ? $"{manager.Name} {manager.Surname}" : " ";
            model.CompanyName = company is not null ? company.Name : " ";
			model.DepartmentName = department is not null ? department.Name : " ";
			

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

		public async Task<List<CompanyManagerVM>> GetUnconfirmedManager()
		{
			List<CompanyManagerVM> companyManagerVM = await _personelRepository.GetFilteredList(x => new CompanyManagerVM
			{
				Id = x.Id,
				FullName = x.Name + " " + x.Surname,
				Email = x.Company.Email,
				PhoneNumber = x.PhoneNumber,
				CompanyName = x.Company.Name,
				Title = x.Title,
				PersonnelCount = x.Company.PersonnelCount,
			}, x => x.Status != Status.Deleted && x.IsAccountConfirmed == false,
			x => x.OrderByDescending(x => x.CreatedDate),
			x => x.Include(x => x.Company));
			return companyManagerVM;
		}

		public async Task<string[]> GetRoles(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user != null)
			{
				var roles = await _userManager.GetRolesAsync(user);
				return roles.ToArray();
			}

			return new string[0];
		}


		public void Logout(string token)
		{
			throw new NotImplementedException();
		}

		//TODO: bakılacak. hatalı Volkan
		public async Task Update(UpdateProfileDTO model)
		{
			var personel = await _personelRepository.GetDefault(x => x.Id == model.Id);
			//bool isExist = await _personelRepository.Any(x => x.Id == model.Id);



			if (personel is not null)
			{

				personel.Name = model.Name;
				personel.PhoneNumber = model.PhoneNumber;
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
				await _profileImageService.UploadFile(personel.Id.ToString(), model.UploadImage);

				await _personelRepository.Update(personel);
			}
		}

		public async Task<SignInResult> Login(LoginDTO model)
		{
			try
			{
				var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
				if (user.IsAccountConfirmed == false)
					return SignInResult.NotAllowed;

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
					x => new UpdateProfileDTO { BirthDate = x.BirthDate, ManagerId = x.ManagerId, ManagerName = "Yönetici", Name = x.Name, Surname = x.Surname, Nation = x.Nation, Gender = x.Gender, CompanyId = x.CompanyId, CompanyName = x.Company.Name, DepartmentId = x.DepartmentId, DepartmentName = x.Department.Name, BloodType = x.BloodType, PhoneNumber = x.PhoneNumber, RegionId = x.RegionId, CityId = x.CityId, Title = x.Title, RegionName = x.Region.Name, CityName = x.City.Name, Address = x.Address, Email = x.Email, HireDate = x.HireDate, ImagePath = $"{_configuration["BaseStorageUrl"]}/{x.PersonnelPicture.FilePath}", Id = x.Id },
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

		public async Task<string> ConfirmManager(Guid id)
		{
			try
			{
				Personnel personnel = await _personelRepository.GetDefault(x => x.Id == id);
				personnel.IsAccountConfirmed = true;
				await _personelRepository.Update(personnel);
				return personnel.Email;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task DeleteNewRegister(Guid id)
		{
			Personnel personnel = await _personelRepository.GetDefault(x => x.Id == id);
			personnel.DeletedDate = DateTime.Now;
			personnel.Status = Status.Deleted;

			Company company = await _companyRepository.GetDefault(x => x.Id == personnel.CompanyId);
			company.DeletedDate = DateTime.Now;
			company.Status = Status.Deleted;

			await _personelRepository.Delete(personnel);
		}
	}
}
