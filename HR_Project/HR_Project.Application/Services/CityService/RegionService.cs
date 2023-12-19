using AutoMapper;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.CityService
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionService(IMapper mapper, IRegionRepository regionRepository)
        {
            _mapper = mapper;
            _regionRepository = regionRepository;
        }

        public async Task<RegionDTO> GetRegionByIdAsync(int regionId)
        {
            RegionDTO model = new RegionDTO();

            Region region = await _regionRepository.GetDefault(x => x.Id == regionId);

            return _mapper.Map<RegionDTO>(region);
        }
        public async Task<List<RegionDTO>> GetRegions()
        {
            return await _regionRepository.GetFilteredList(x => new RegionDTO
            {
                CityId = x.CityId,
                Name = x.Name,
                City = x.City,
                RegionId = x.Id
            }, x => x.Status != Status.Deleted);
        }
    }
}
