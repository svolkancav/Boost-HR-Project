using HR_Project.Common;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace HR_Project.Presentation.Controllers
{
	public class AdminController : BaseController
	{
		private readonly IAPIService _apiService;
		private readonly IEmailService _emailService;

        public AdminController(IAPIService apiService, IEmailService emailService)
        {
            _apiService = apiService;
            _emailService = emailService;
        }


        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10)
		{
			if (!string.IsNullOrEmpty(searchText))
			{
				List<CompanyManagerVM> companyManager = await _apiService.GetAsync<List<CompanyManagerVM>>("personnel/GetUnconfirmed", HttpContext.Request.Cookies["access-token"]);
				List<CompanyManagerVM> selectedCM = companyManager.Where(x => x.CompanyName.ToLower().Contains(searchText.ToLower())).ToList();

				return View(selectedCM.ToPagedList(pageNumber, pageSize));
			}
			else
			{
				List<CompanyManagerVM> companyManager = await _apiService.GetAsync<List<CompanyManagerVM>>("personnel/GetUnconfirmed", HttpContext.Request.Cookies["access-token"]);
				return View(companyManager.ToPagedList(pageNumber, pageSize));
			}

		}

		[HttpPost]
		public async Task<IActionResult> Confirm([FromForm] Guid id)
		{
			try
			{
				var response= await _apiService.PostAsync<object, ConfirmManagerResponse > ($"personnel/ConfirmManager", id, HttpContext.Request.Cookies["access-token"]);
				Toastr("success", "Başarılı bir şekilde onaylandı");
				
				await _emailService.SendEmailRegisterAsync(response.Email, "HR Hesap Onayı", "İyi günler sayın yetkili hesabınız onaylanmıştır");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Toastr("error", $"Onay sırasında hata oluştu : {ex.Message}");
				return RedirectToAction("Index");
			}
			
		}
		[HttpPost]
		public async Task<IActionResult> Delete([FromForm] string id)
		{
			try
			{
				await _apiService.DeleteAsync<object>($"personnel", id, HttpContext.Request.Cookies["access-token"]);
				Toastr("success", "Başarılı bir şekilde silindi");

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				Toastr("error", $"Silme işleminde hata oluştu : {ex.Message}");
				return RedirectToAction("Index");
			}
		}
	}
}
