using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HR_Project.Presentation.ViewComponents
{
	public class ExpenseCardComponent: ViewComponent
	{
		private readonly IAPIService _apiService;

		public ExpenseCardComponent(IAPIService apiService)
		{
			_apiService = apiService;
		}

		//public async Task<IViewComponentResult> InvokeAsync()
		//{
		//	var expenses = await _apiService.GetAsync<List<ExpenseVM>>($"Expense", HttpContext.Request.Cookies["access-token"]);
		//	return await Task.FromResult(View());
		//}


        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<MasterExpenseVM> pendingExpense = await _apiService.GetAsync<List<MasterExpenseVM>>($"Expense/{ConditionType.Pending}", HttpContext.Request.Cookies["access-token"]);

            int pendingExpenseCount = pendingExpense.Count;

            return new ContentViewComponentResult(pendingExpenseCount.ToString());
        }
    }
}
