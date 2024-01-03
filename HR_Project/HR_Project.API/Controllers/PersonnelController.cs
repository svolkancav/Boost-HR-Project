using System.Security.Claims;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Application.Services.AdvanceService;
using HR_Project.Application.Services.FileService;
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
		private readonly IProfileImageService _profileImageService;
        private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;

        public PersonnelController(IPersonnelService personnelService, IProfileImageService profileImageService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _personnelService = personnelService;
            _profileImageService = profileImageService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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


        [HttpGet]
		[Route("GetUnconfirmed")]
		public async Task<IActionResult> GetUnconfirmed()
		{
			var personnels = await _personnelService.GetUnconfirmedManager();
			
			return Ok(personnels);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> ConfirmManager([FromBody] Guid id)
		{
			string email = await _personnelService.ConfirmManager(id);
            return Ok(new {Email=email});
        }

		[HttpPost]
		[Route("[action]")]
		public async Task<IActionResult> DeleteNewRegister([FromBody] Guid id)
		{
            await _personnelService.DeleteNewRegister(id);
            
            return Ok();
        }

		[HttpPost]
		public async Task<IActionResult> Create(CreateProfileDTO model)
		{
			await _personnelService.Create(model);
			return Ok();
		}


		[HttpPut]
		public async Task<IActionResult> Update(UpdateProfileDTO model)
		{
			await _personnelService.Update(model);
			return Ok(model);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await _personnelService.Delete(id);
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> UploadImage()
		{
			string id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var a = Request.Form.Files.First();

            await _profileImageService.UploadFile(id, Request.Form.Files.First());
			return Ok();
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetImage(string id)
		{
			var img= await _profileImageService.GetFileById(id);
			return Ok(new
			{
				ImagePath = $"{img}"
			});
		}

		[HttpDelete("[action]/{id}")]
		public async Task<IActionResult> DeleteImage(string id)
		{
			await _profileImageService.DeleteFile(id);
			return Ok();
		}

	}
}
