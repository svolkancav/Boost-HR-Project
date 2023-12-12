using System;
using System.Collections.Generic;
using System.Linq;
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

namespace HR_Project.Application.Services.AdvanceService
{
    public class AdvanceServise : IAdvanceServise
    {
        private readonly IAdvanceRepository _advanceRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Personnel> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdvanceServise(IAdvanceRepository advanceRepository, IMapper mapper, UserManager<Personnel> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _advanceRepository = advanceRepository;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Create(CreateAdvanceDTO model)
        {
            Personnel personnel = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            
            Advance advance = _mapper.Map<Advance>(model);
            advance.Status = Status.Inserted;
            advance.CreatedDate = DateTime.Now;
            advance.PersonnelId = personnel.Id;

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
                Amount = x.Amount,
                LastPaidDate = x.LastPaidDate,
                Condition = x.Condition,
                Reason = x.Reason,
            }, x => x.Status != Status.Deleted);
        }

        public async Task<List<AdvanceVM>> GetByCondition(ConditionType condition)
        {
            return await _advanceRepository.GetFilteredList(x => new AdvanceVM
            {
                Amount = x.Amount,
                LastPaidDate = x.LastPaidDate,
                Condition = x.Condition,
                Reason = x.Reason,
            }, x => x.Status != Status.Deleted && x.Condition == condition);
        }

        public async Task Update(UpdateAdvanceDTO model)
        {
            // TODO: Implement Update
        }
    }
}
