using HR_Project.Application.IoC.Models.DTOs;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10)
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

        public async Task<IActionResult> Create()
        {
            List<CityDTO> cities = await _apiService.GetAsync<List<CityDTO>>("city", HttpContext.Request.Cookies["access-token"]);
            List<RegionDTO> regionList = await _apiService.GetAsync<List<RegionDTO>>("region", HttpContext.Request.Cookies["access-token"]);
            List<CreateProfileDTO> personnelList = await _apiService.GetAsync<List<CreateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
            List<DepartmentDTO> departments = await _apiService.GetAsync<List<DepartmentDTO>>("department", HttpContext.Request.Cookies["access-token"]);

            CreateProfileDTO model = new CreateProfileDTO();
            model.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            model.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();
            model.Departments = departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();

            model.Managers = personnelList
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(model);


        }
        //TODO: PErsonel oluştururken şifre verilmeyecek.
        [HttpPost]
        public async Task<IActionResult> Create(CreateProfileDTO model)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var personnel = await _apiService.GetByIdAsync<CreateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);
                model.CompanyId = personnel.CompanyId;
                await _apiService.PostAsync<CreateProfileDTO, CreateProfileDTO>("personnel", model, HttpContext.Request.Cookies["access-token"]);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");

        }

        public async Task<IActionResult> Profil()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);

            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");

            personnel.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            personnel.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(personnel);
        }

        public async Task<IActionResult> Update(string id)
        {

            var personnel = await _apiService.GetByIdAsync<UpdateProfileDTO>("personnel", id, HttpContext.Request.Cookies["access-token"]);

            List<CityDTO> cities = await _apiService.GetAsyncWoToken<List<CityDTO>>("city");
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");

            personnel.CityList = cities.Select(c => new SelectListItem
            {
                Value = c.CityId.ToString(),
                Text = c.Name
            })
                .ToList();
            personnel.Regions = regionList
                //.Where(d => d.CityId == personnel.CityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();


            return View(personnel);
        }

        //TODO: Güncelleme hatalı
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _apiService.DeleteAsync<PersonelDTO>($"personnel", id, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde silindi.");
                return RedirectToAction("ListPersonnel");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt silinirken hata oluştu : {ex.Message}");
                return RedirectToAction("ListPersonnel");
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetRegions(int cityId)
        {
            List<RegionDTO> regionList = await _apiService.GetAsyncWoToken<List<RegionDTO>>("region");
            var regions = regionList
                .Where(d => d.CityId == cityId)
                .Select(d => new SelectListItem
                {
                    Value = d.RegionId.ToString(),
                    Text = d.Name
                })
                .ToList();

            return Json(regions);
        }

        [HttpGet]
        public async Task<JsonResult> GetManagersByDepartment(int departmentId)
        {
            List<UpdateProfileDTO> personnelList = await _apiService.GetAsync<List<UpdateProfileDTO>>("personnel", HttpContext.Request.Cookies["access-token"]);
            var managers = personnelList
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new SelectListItem
                {
                    Value = d.ManagerId.ToString(),
                    Text = d.ManagerName
                })
                .ToList();

            return Json(managers);
        }
    }
}
