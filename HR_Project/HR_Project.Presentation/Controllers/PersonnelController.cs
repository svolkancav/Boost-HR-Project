using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace HR_Project.Presentation.Controllers
{
    public class PersonnelController : BaseController
    {
        private readonly IAPIService _apiService;
       

        public PersonnelController(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> ListPersonnel(string searchText, int pageNumber = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                List<PersonelVM> personnels = await _apiService.GetAsync<List<PersonelVM>>("personnel", HttpContext.Request.Cookies["access-token"]);
                List<PersonelVM> selectedPersonnels = personnels.Where(x => x.Name.ToLower().Contains(searchText.ToLower()) || x.Surname.ToString().ToLower().Contains(searchText.ToLower())).ToList();

                return View(selectedPersonnels.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<PersonelVM> personnels = await _apiService.GetAsync<List<PersonelVM>>("personnel", HttpContext.Request.Cookies["access-token"]);
                return View(personnels.ToPagedList(pageNumber, pageSize));
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonelDTO model)
        {
            await _apiService.PostAsync<PersonelDTO, PersonelDTO>("personnel", model, HttpContext.Request.Cookies["access-token"]); 
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            UpdateProfileDTO personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>($"personnel",id, HttpContext.Request.Cookies["access-token"]);
            return View(personnel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProfileDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateProfileDTO>("personnel", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                return RedirectToAction("Update");

            }

            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                return View(model);
            }

        }

        // silme için
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<PersonelDTO>($"personnel", id, HttpContext.Request.Cookies["access-token"]);
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
