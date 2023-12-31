using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HR_Project.Presentation.ViewComponents
{
	public class AdvanceCardComponent: ViewComponent
	{
		private readonly IAPIService _apiService;

		public AdvanceCardComponent(IAPIService apiService)
		{
			_apiService = apiService;
		}

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //	List<AdvanceVM> advances = await _apiService.GetAsync<List<AdvanceVM>>($"Advance/{ConditionType.Pending}", HttpContext.Request.Cookies["access-token"]);
        //	return await Task.FromResult(View(advances));
        //}

        public async Task<IViewComponentResult> InvokeAsync(bool ownList)
        {
            if (ownList)
            {
                List<AdvanceVM> pendingAdvances = await _apiService.GetAsync<List<AdvanceVM>>($"Advance/{ConditionType.Pending}", HttpContext.Request.Cookies["access-token"]);

                int pendingAdvanceCount = pendingAdvances.Count;

                return new ContentViewComponentResult(pendingAdvanceCount.ToString());
            }
            else
            {
                List<PersonnelsListDTO> pendingPersonnelAdvances = await _apiService.GetAsync<List<PersonnelsListDTO>>($"Advance/GetPendingAdvance", HttpContext.Request.Cookies["access-token"]);

                int pendingAdvanceCount = pendingPersonnelAdvances.Where(x => x.Advances != null).SelectMany(x => x.Advances).Count();
                return new ContentViewComponentResult(pendingAdvanceCount.ToString());
            }
            
        }
    }
}
