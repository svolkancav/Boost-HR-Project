using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace HR_Project.Presentation.Controllers
{
    public class AbsenceController : BaseController
    {
        private readonly IAPIService _apiService;

        public AbsenceController(IAPIService apiService)
        {
            _apiService = apiService;
        }
        // listelemek için
        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10, string sortColumn = "", string sortOrder = "")
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                List<AbsenceVM> absences = await _apiService.GetAsync<List<AbsenceVM>>("absence", HttpContext.Request.Cookies["access-token"]);

                List<AbsenceVM> selectedAbsences = absences.Where(x => x.Reason.ToLower().Contains(searchText.ToLower()) || x.LeaveTypes.ToString().ToLower().Contains(searchText.ToLower())).ToList();
				selectedAbsences = ApplySorting(selectedAbsences.AsQueryable(), sortColumn, sortOrder).ToList();
				return View(selectedAbsences.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<AbsenceVM> absences = await _apiService.GetAsync<List<AbsenceVM>>("absence", HttpContext.Request.Cookies["access-token"]);
				absences = ApplySorting(absences.AsQueryable(), sortColumn, sortOrder).ToList();
				return View(absences.ToPagedList(pageNumber, pageSize));
            }

        }

		private IQueryable<AbsenceVM> ApplySorting(IQueryable<AbsenceVM> absenceList, string sortColumn, string sortOrder)
		{
			switch (sortColumn)
			{
				case "CreateDate":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.CreatedDate) : absenceList.OrderByDescending(p => p.CreatedDate);
					break;
				case "LeaveTypes":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.LeaveTypes) : absenceList.OrderByDescending(p => p.LeaveTypes);
					break;
				case "Reason":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.Reason) : absenceList.OrderByDescending(p => p.Reason);
					break;
				case "StartDate":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.StartDate) : absenceList.OrderByDescending(p => p.StartDate);
					break;
				case "EndDate":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.EndDate) : absenceList.OrderByDescending(p => p.EndDate);
					break;
				case "AbsenceDuration":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.AbsenceDuration) : absenceList.OrderByDescending(p => p.AbsenceDuration);
					break;
				case "Condition":
					absenceList = sortOrder == "asc" ? absenceList.OrderBy(p => p.Condition) : absenceList.OrderByDescending(p => p.Condition);
					break;

				// Add cases for other columns as needed
				default:
					// Default sorting by Name in ascending order
					absenceList = absenceList.OrderByDescending(p => p.CreatedDate);
					break;
			}

			return absenceList;
		}

		// ekleme için
		public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AbsenceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.PostAsync<AbsenceDTO, AbsenceDTO>("Absence", model, HttpContext.Request.Cookies["access-token"]); //_apiService.PostAsync<AbsenceDTO,AbsenceDTO>... ikinci tip geri dönüş tipi. Ama API den geri dönüş modeli göndermiyoruz.
                
                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");
                
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");

                return View(model);
            }

        }

        // güncelleme için
        public async Task<IActionResult> Update(string id)
        {
            UpdateAbsenceDTO absence = await _apiService.GetByIdAsync<UpdateAbsenceDTO>($"absence/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(absence);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAbsenceDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateAbsenceDTO>("absence", model, HttpContext.Request.Cookies["access-token"]);

                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncellenirken hata oluştu : {ex.Message}");

                return View(model);
            }
        }


        // TODO: Silme hatalı Sultan- Silme işleminde API uyarısı veriyor ama siliyor.
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<UpdateAbsenceDTO>($"absence", id, HttpContext.Request.Cookies["access-token"]);
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
