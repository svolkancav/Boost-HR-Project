using HR_Project.Application.Services.CityService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int regionId)
        {
            var region = await _regionService.GetRegionByIdAsync(regionId);
            return Ok(region);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var regions = await _regionService.GetRegions();
            return Ok(regions);
        }

    }
}
