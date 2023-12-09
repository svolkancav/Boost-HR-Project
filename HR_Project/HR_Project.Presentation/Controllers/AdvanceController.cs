using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.Controllers
{
    public class AdvanceController : Controller
    {
        private readonly IAPIService _apiService;

        public AdvanceController(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            List<AdvanceVM> advances = await _apiService.GetAsync<List<AdvanceVM>>("advance", HttpContext.Request.Cookies["access-token"]);

            return View(advances);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvanceDTO model)
        {
            await _apiService.PostAsync<CreateAdvanceDTO, CreateAdvanceDTO>("advance", model, HttpContext.Request.Cookies["access-token"]); 
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int id)
        {
            AdvanceVM advance = await _apiService.GetAsync<AdvanceVM>($"advance/{id}", HttpContext.Request.Cookies["access-token"]);
            return View(advance);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAdvanceDTO model)
        {
            await _apiService.UpdateAsync<UpdateAdvanceDTO>("advance", model, HttpContext.Request.Cookies["access-token"]);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _apiService.DeleteAsync<AbsenceDTO>($"advance", id, HttpContext.Request.Cookies["access-token"]);
            return RedirectToAction("Index");
        }
    }
}
