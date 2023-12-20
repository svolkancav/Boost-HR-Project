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
        private readonly IAdvanceServise _advanceService;
        private readonly IAbsenceService _absenceService;
        private readonly IExpenseService _expenseService;

        public DashboardController(
            IAPIService apiService,
            IAdvanceServise advanceService,
            IAbsenceService absenceService,
            IExpenseService expenseService)
        {
            _apiService = apiService;
            _advanceService = advanceService;
            _absenceService = absenceService;
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<AdvanceVM> advances = await _advanceService.GetAdvances();
                List<AbsenceVM> absences = await _absenceService.GetAbsences();
                List<ExpenseVM> expenses = await _expenseService.GetExpenses();

                ViewBag.Advances = advances;
                ViewBag.Absences = absences;
                ViewBag.Expenses = expenses;

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
