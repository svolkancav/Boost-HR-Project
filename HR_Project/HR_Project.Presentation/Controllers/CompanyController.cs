using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using X.PagedList;

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

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.PostAsync<CreateCompanyDTO, CreateCompanyDTO>("Company", model, HttpContext.Request.Cookies["access-token"]); 

                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");

                return View(model);
            }

        }


        public async Task<IActionResult> Update(string id)
        {
            UpdateCompanyDTO company = await _apiService.GetByIdAsync<UpdateCompanyDTO>($"company/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCompanyDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateCompanyDTO>("company", model, HttpContext.Request.Cookies["access-token"]);

                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncellenirken hata oluştu : {ex.Message}");

                return View(model);
            }
        }

        // silme için
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<UpdateCompanyDTO>($"company", id, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde silindi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt silinirken hata oluştu : {ex.Message}");
                return RedirectToAction("Index");
            }
        }

    }
}
