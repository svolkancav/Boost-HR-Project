
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.ViewComponents
{
    public class AbsenceCardComponent : ViewComponent
    {
        private readonly IAPIService _apiService;

        public AbsenceCardComponent(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<AbsenceVM> absences = await _apiService.GetAsync<List<AbsenceVM>>($"Absence/{ConditionType.Pending}", HttpContext.Request.Cookies["access-token"]);

            return await Task.FromResult(View(absences));
        }
    }
}
