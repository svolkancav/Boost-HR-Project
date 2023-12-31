using HR_Project.Application.Services.AdvanceService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdvanceController : ControllerBase
	{
		private readonly IAdvanceServise _advanceService;

		public AdvanceController(IAdvanceServise advanceService)
		{
			_advanceService = advanceService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var advances =await _advanceService.GetAdvances();
			return Ok(advances);
		}

		[HttpGet]
		[Route("getbyid/{id}")]
		public async Task<IActionResult> GetById(string id)
		{
            var advance =await _advanceService.GetById(id);
            return Ok(advance);
        }

		[HttpGet]
		[Route("{condition}")]
		public async Task<IActionResult> GetByCondition(ConditionType condition)
		{
			var advances =await _advanceService.GetByCondition(condition);
			return Ok(advances);
		}
		[HttpGet]
		[Route("GetPendingAdvance")]
		public async Task<IActionResult> GetPendingAdvance()
		{
            var advances = await _advanceService.GetPendingAdvance();
            return Ok(advances);
        }

		[HttpPost]
		public async Task<IActionResult> Create(CreateAdvanceDTO model)
		{
			await _advanceService.Create(model);
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Update(UpdateAdvanceDTO model)
		{
			await _advanceService.Update(model);
			return Ok();
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _advanceService.Delete(id);
			return Ok();
		}
	}
}
