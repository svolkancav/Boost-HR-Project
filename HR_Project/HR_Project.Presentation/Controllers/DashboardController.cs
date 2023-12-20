using HR_Project.Application.Services.AbsenceService;
using HR_Project.Application.Services.AdvanceService;
using HR_Project.Application.Services.ExpenseService;
using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Project.Presentation.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IAPIService _apiService;

        public DashboardController(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Dashboard verileri alınırken bir hata oluştu: {ex.Message}";
                return View("Error");
            }
        }
    }
}
