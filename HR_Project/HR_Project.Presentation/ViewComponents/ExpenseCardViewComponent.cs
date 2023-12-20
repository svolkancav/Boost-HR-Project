using HR_Project.Common.Models.VMs;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.ViewComponents
{
	public class ExpenseCardViewComponent: ViewComponent
	{
		private readonly IAPIService _apiService;

		public ExpenseCardViewComponent(IAPIService apiService)
		{
			_apiService = apiService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var expenses = await _apiService.GetAsync<List<ExpenseVM>>($"Expense", HttpContext.Request.Cookies["access-token"]);
			return await Task.FromResult(View());
		}
	}
}
