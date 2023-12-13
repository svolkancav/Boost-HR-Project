using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Application.Services.AdvanceService;
using HR_Project.Application.Services.PersonelServices;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _personnelService;

        public PersonnelController(IPersonnelService personnelService)
        {
            _personnelService = personnelService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var personnels = await _personnelService.GetPersonels();
            
            return Ok(personnels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var personnel = await _personnelService.GetById(id);
            return Ok(personnel);   
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonelDTO model)
        {
            await _personnelService.Create(model);
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Update(PersonelDTO model)
        {
            await _personnelService.Update(model);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _personnelService.Delete(id);
            return Ok();
        }

    }
}
