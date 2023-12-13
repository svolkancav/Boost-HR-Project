using HR_Project.Application.Services.AbsenceService;
using HR_Project.Application.Services.AdvanceService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenceController : ControllerBase
    {
        private readonly IAbsenceService _absenceService;

        public AbsenceController(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var absences = await _absenceService.GetAbsences();
            return Ok(absences);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var absence = await _absenceService.GetById(id);
            return Ok(absence);
        }

        [HttpGet]
        [Route("{condition}")]
        public async Task<IActionResult> GetByCondition(ConditionType condition)
        {
            var absences = await _absenceService.GetByCondition(condition);
            return Ok(absences);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AbsenceDTO model)
        {
            await _absenceService.Create(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAbsenceDTO model)
        {
            await _absenceService.Update(model);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _absenceService.Delete(id);
            return Ok();
        }
    }
}
