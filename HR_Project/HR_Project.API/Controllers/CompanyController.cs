using HR_Project.Application.Services.CompanyService;
using HR_Project.Common.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;


        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCompanyDTO model)
        {
            await _companyService.Create(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update (UpdateCompanyDTO model) 
        {
            await _companyService.Update(model);
            return Ok();
        
        }


        [HttpDelete]
        [Route ("{id}")]
        public async Task<IActionResult> Delete(int id) 
        { 
            await _companyService.Delete(id);   
            return Ok();
        
        }





    }
}
