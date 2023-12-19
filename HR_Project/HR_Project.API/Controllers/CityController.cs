using HR_Project.Application.Services.CityService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cities = await _cityService.GetCities();
            return Ok(cities);
        }
    }
}
