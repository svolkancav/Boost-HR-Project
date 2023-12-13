using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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
        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 3)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                List<AbsenceVM> absences = await _apiService.GetAsync<List<AbsenceVM>>("absence", HttpContext.Request.Cookies["access-token"]);
                List<AbsenceVM> selectedAbsences = absences.Where(x => x.Reason.ToLower().Contains(searchText.ToLower()) || x.LeaveTypes.ToString().ToLower().Contains(searchText.ToLower())).ToList();

                return View(selectedAbsences.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<AbsenceVM> absences = await _apiService.GetAsync<List<AbsenceVM>>("absence", HttpContext.Request.Cookies["access-token"]);
                return View(absences.ToPagedList(pageNumber, pageSize));
            }

        }

        // ekleme için
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AbsenceDTO model)
        {
            await _apiService.PostAsync<AbsenceDTO, AbsenceDTO>("Absence", model, HttpContext.Request.Cookies["access-token"]); //_apiService.PostAsync<AbsenceDTO,AbsenceDTO>... ikinci tip geri dönüş tipi. Ama API den geri dönüş modeli göndermiyoruz.
            return RedirectToAction("Index");
        }

        // güncelleme için
        public async Task<IActionResult> Update(string id)
        {
            UpdateAbsenceDTO absence = await _apiService.GetByIdAsync<UpdateAbsenceDTO>($"absence/getbyid",id, HttpContext.Request.Cookies["access-token"]);
            return View(absence);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAbsenceDTO model)
        {
            await _apiService.UpdateAsync<UpdateAbsenceDTO>("absence", model, HttpContext.Request.Cookies["access-token"]);
            return RedirectToAction("Index");
        }

        // silme için
        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync<UpdateAbsenceDTO>($"absence", id, HttpContext.Request.Cookies["access-token"]);
            return RedirectToAction("Index");
        }
    }
}
