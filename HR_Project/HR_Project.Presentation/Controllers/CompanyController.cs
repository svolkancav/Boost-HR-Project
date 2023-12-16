using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;

namespace HR_Project.Presentation.Controllers
{
	public class CompanyController : BaseController
	{
		private readonly IAPIService _apiService;

		public CompanyController(IAPIService apiService)
		{
			_apiService = apiService;
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateCompanyDTO model)
		{
			await _apiService.PostAsync<CreateCompanyDTO, CreateCompanyDTO>("company", model, HttpContext.Request.Cookies["access-token"]);
			return RedirectToAction("Index");
		}
	}
}
