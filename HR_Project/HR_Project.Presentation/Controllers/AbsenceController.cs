using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly IAPIService _apiService;

        public AbsenceController(IAPIService apiService)
        {
            _apiService = apiService;
        }
        // listelemek için
        public async Task<IActionResult> Index()
        {
            List<AbsenceVM> absences =await _apiService.GetAsync<List<AbsenceVM>>("absence", HttpContext.Request.Cookies["access-token"]);

            return View(absences);
        }

        // ekleme için
        public IActionResult Create()
        {
			return View();
		}
        [HttpPost]
		public async Task<IActionResult> Create(AbsenceDTO model)
        {
			await _apiService.PostAsync<AbsenceDTO,AbsenceDTO>("absence", model, HttpContext.Request.Cookies["access-token"]); //_apiService.PostAsync<AbsenceDTO,AbsenceDTO>... ikinci tip geri dönüş tipi. Ama API den geri dönüş modeli göndermiyoruz.
			return RedirectToAction("Index");
		}

		// güncelleme için
		public async Task<IActionResult> Update(int id)
        {
			AbsenceVM absence = await _apiService.GetAsync<AbsenceVM>($"absence/{id}", HttpContext.Request.Cookies["access-token"]);
			return View(absence);
		}

        [HttpPost]
		public async Task<IActionResult> Update(AbsenceDTO model)
        {
			await _apiService.UpdateAsync<AbsenceDTO>("absence", model, HttpContext.Request.Cookies["access-token"]);
			return RedirectToAction("Index");
		}

		// silme için
		public async Task<IActionResult> Delete(int id)
        {
			await _apiService.DeleteAsync<AbsenceDTO>($"absence",id, HttpContext.Request.Cookies["access-token"]);
			return RedirectToAction("Index");
		}
    }
}
