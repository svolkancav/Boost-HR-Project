﻿using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HR_Project.Application.Services.AdvanceService
{
    public class AdvanceServise : IAdvanceServise
    {
        private readonly IAdvanceRepository _advanceRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Personnel> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private Personnel currentPersonnel;
        private readonly IPersonelRepository _personnelRepository;

        public AdvanceServise(IAdvanceRepository advanceRepository, IMapper mapper, UserManager<Personnel> userManager, IHttpContextAccessor httpContextAccessor, IPersonelRepository personnelRepository)
        {
            _advanceRepository = advanceRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
			currentPersonnel = _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;
            _personnelRepository = personnelRepository;
        }

        public async Task Create(CreateAdvanceDTO model)
        {
            
            
            Advance advance = _mapper.Map<Advance>(model);
            advance.Status = Status.Inserted;
            advance.CreatedDate = DateTime.Now;
            advance.PersonnelId = currentPersonnel.Id;
            advance.Condition = ConditionType.Pending;

            await _advanceRepository.Create(advance);
        }

        public async Task Delete(int id)
        {
            Advance advance = await _advanceRepository.GetDefault(x => x.Id == id);
            if (id == 0)
            {
                throw new ArgumentException("Id 0 Olamaz!");

            }
            else if (advance == null)
            {
                throw new ArgumentException("Böyle bir yazar mevcut değil!");
            }

            advance.DeletedDate = DateTime.Now;
            advance.Status = Status.Deleted;
            await _advanceRepository.Delete(advance);
        }

        public async Task<List<AdvanceVM>> GetAdvances()
        {
            return await _advanceRepository.GetFilteredList(x => new AdvanceVM
            {
                Id = x.Id,
                Amount = x.Amount,
                LastPaidDate = x.LastPaidDate,
                Condition = x.Condition,
                Reason = x.Reason,
                Currency = x.Currency,
                CreatedDate = x.CreatedDate,
            }, x => x.Status != Status.Deleted && x.PersonnelId == currentPersonnel.Id);
        }

        public async Task<List<AdvanceVM>> GetByCondition(ConditionType condition)
        {
            return await _advanceRepository.GetFilteredList(x => new AdvanceVM
            {
                Id = x.Id,
                Amount = x.Amount,
                LastPaidDate = x.LastPaidDate,
                Condition = x.Condition,
                Reason = x.Reason,
                Currency = x.Currency
            }, x => x.Status != Status.Deleted && x.Condition == condition && x.PersonnelId == currentPersonnel.Id);
        }

        public async Task<UpdateAdvanceDTO> GetById(string id)
        {
            UpdateAdvanceDTO model = new UpdateAdvanceDTO();

            Advance advance= await _advanceRepository.GetDefault(x => x.Id == Convert.ToInt32(id));

            return _mapper.Map<UpdateAdvanceDTO>(advance);
        }

        public async Task<List<PersonnelsListDTO>> GetPendingAdvance()
        {
            var personnels = await _personnelRepository.GetFilteredList(x => new PersonnelsListDTO
            {
                Id = x.Id,
                Advances = _mapper.Map<List<AdvanceVM>>(x.Advances.Where(x => x.Status != Status.Deleted && x.Condition == ConditionType.Pending)),
            },
                x => x.Status != Status.Deleted && x.ManagerId == currentPersonnel.Id);

            return personnels;
        }

        public async Task Update(UpdateAdvanceDTO model)
        {
            Advance advance = await _advanceRepository.GetDefault(x => x.Id == model.Id);

            advance.Amount = model.Amount;
            advance.Reason = model.Reason;
            advance.LastPaidDate = model.LastPaidDate;
            advance.ModifiedDate = DateTime.Now;
            advance.Status = Status.Updated;

            await _advanceRepository.Update(advance);

        }
    }
}
