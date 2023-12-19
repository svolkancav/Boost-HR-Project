using HR_Project.Common.Models.DTOs;

namespace HR_Project.Application.Services.CityService
{
    public interface IRegionService 
    {
        Task<RegionDTO> GetRegionByIdAsync(int regionId);
        Task<List<RegionDTO>> GetRegions();
    }
}
