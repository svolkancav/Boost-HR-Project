using HR_Project.Application.Services.CompanyService;
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




	}
}
