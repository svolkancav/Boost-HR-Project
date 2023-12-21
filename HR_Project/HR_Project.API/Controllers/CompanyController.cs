using HR_Project.Application.Services.CompanyService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly UserManager<Personnel> _userManager;


        public CompanyController(ICompanyService companyService, UserManager<Personnel> userManager)
        {
            _companyService = companyService;
            _userManager = userManager;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var companies = await _companyService.GetCompanies();
            return Ok(companies);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var company = await _companyService.GetById(id);
            return Ok(company);
        }

    }
}
