using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HR_Project.Application.Services.AbsenceService
{
	public class AbsenceService : IAbsenceService
	{
		private readonly IAbsenceRepository _absenceRepository;
		private readonly IMapper _mapper;
		private readonly UserManager<Personnel> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private Personnel currentPersonnel;
		private readonly IPersonnelRepository _personnelRepository;


		public AbsenceService(IAbsenceRepository absenceRepository, IMapper mapper, UserManager<Personnel> userManager, IHttpContextAccessor httpContextAccessor, IPersonnelRepository personnelRepository)
		{
			_absenceRepository = absenceRepository;
			_mapper = mapper;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			currentPersonnel = _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;
			_personnelRepository = personnelRepository;
		}

		public async Task Create(AbsenceDTO model)
		{
			Absence absence = _mapper.Map<Absence>(model);
			absence.Status = Status.Inserted;
			absence.CreatedDate = DateTime.Now;
			absence.PersonnelId = currentPersonnel.Id;
			absence.Condition = ConditionType.Pending;
			try
			{
				await _absenceRepository.Create(absence);

			}
			catch (Exception message)
			{

				throw message;
			}

		}

		public async Task Delete(int id)
		{
			Absence absence = await _absenceRepository.GetDefault(x => x.Id == id);
			if (id == 0)
			{
				throw new ArgumentException("Id 0 Olamaz!");

			}
			else if (absence == null)
			{
				throw new ArgumentException("Böyle bir izin mevcut değil!");
			}

			absence.DeletedDate = DateTime.Now;
			absence.Status = Status.Deleted;
			await _absenceRepository.Delete(absence);
		}

		public async Task<List<AbsenceVM>> GetAbsences()
		{
			return await _absenceRepository.GetFilteredList(x => new AbsenceVM
			{
				Id = x.Id,
				Reason = x.Reason,
				LeaveTypes = x.LeaveTypes,
				AbsenceDuration = x.AbsenceDuration,
				StartDate = x.StartDate,
				EndDate = x.EndDate,
				Condition = x.Condition,
			}, x => x.Status != Status.Deleted && x.PersonnelId == currentPersonnel.Id);
		}

		public async Task<List<AbsenceVM>> GetByCondition(ConditionType condition)
		{
			return await _absenceRepository.GetFilteredList(x => new AbsenceVM
			{
				Id = x.Id,
				Reason = x.Reason,
				LeaveTypes = x.LeaveTypes,
				AbsenceDuration = x.AbsenceDuration,
				StartDate = x.StartDate,
				EndDate = x.EndDate,
			}, x => x.Status != Status.Deleted && x.Condition == condition);
		}

		public async Task<UpdateAbsenceDTO> GetById(string id)
		{
			UpdateAbsenceDTO model = new UpdateAbsenceDTO();

			Absence absence = await _absenceRepository.GetDefault(x => x.Id == Convert.ToInt32(id));


			var a = _mapper.Map<UpdateAbsenceDTO>(absence);
			return a;

		}

		public async Task<List<AbsenceVM>> GetPendingAbsence()
		{
			List<Personnel> personnels = await _personnelRepository.GetDefaults(x => x.Status != Status.Deleted&&x.ManagerId==currentPersonnel.Id);

			
			return await _absenceRepository.GetFilteredList(x => new AbsenceVM
			{
				Id = x.Id,
				Reason = x.Reason,
				LeaveTypes = x.LeaveTypes,
				AbsenceDuration = x.AbsenceDuration,
				StartDate = x.StartDate,
				EndDate = x.EndDate,
			}, x => x.Status != Status.Deleted );
		}

		public async Task Update(UpdateAbsenceDTO model)
		{
			Absence abcense = await _absenceRepository.GetDefault(x => x.Id == model.Id);

			abcense.Reason = model.Reason;
			abcense.LeaveTypes = model.LeaveTypes;
			abcense.StartDate = model.StartDate;
			abcense.EndDate = model.EndDate;
			abcense.AbsenceDuration = model.AbsenceDuration;
			abcense.ModifiedDate = DateTime.Now;
			abcense.Status = Status.Updated;

			await _absenceRepository.Update(abcense);

		}
	}
}
