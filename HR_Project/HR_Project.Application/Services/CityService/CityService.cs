using AutoMapper;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.CityService
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository )
        {
            _cityRepository = cityRepository;

        }


        public async Task<List<CityDTO>> GetCities()
        {
            return await _cityRepository.GetFilteredList(x => new CityDTO
            {
                CityId = x.Id,
                Name = x.Name,
                Regions = x.Regions
            }, x => x.Status != Status.Deleted);
        }
    }
}
