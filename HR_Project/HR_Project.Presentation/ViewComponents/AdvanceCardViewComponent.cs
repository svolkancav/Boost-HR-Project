using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.ViewComponents
{
	public class AdvanceCardComponent: ViewComponent
	{
		private readonly IAPIService _apiService;

		public AdvanceCardComponent(IAPIService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<AdvanceVM> advances = await _apiService.GetAsync<List<AdvanceVM>>($"Advance/{ConditionType.Pending}", HttpContext.Request.Cookies["access-token"]);
			return await Task.FromResult(View(advances));
		}
	}
}
