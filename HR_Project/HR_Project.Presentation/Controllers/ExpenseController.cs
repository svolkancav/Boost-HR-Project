using System.Security.Claims;
using HR_Project.Common.Extensions;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace HR_Project.Presentation.Controllers
{
    public class ExpenseController : BaseController
    {
        private readonly IAPIService _apiService;

        public ExpenseController(IAPIService apiService)
        {
			_apiService = apiService;
        }

        public async Task<IActionResult> Index(string searchText, int pageNumber = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(searchText))
            {

                List<MasterExpenseVM> expense_MasterExpenses = await _apiService.GetAsync<List<MasterExpenseVM>>("Expense", HttpContext.Request.Cookies["access-token"]);

                return View(expense_MasterExpenses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<MasterExpenseVM> expense_MasterExpenses = await _apiService.GetAsync<List<MasterExpenseVM>>("Expense", HttpContext.Request.Cookies["access-token"]);
                return View(expense_MasterExpenses.ToPagedList(pageNumber, pageSize));
            }

        }


        public IActionResult Create()
        {
			ViewBag.EnumValues = new
			{
				ExpenseTypes = EnumHelper.GetEnumSelectList<ExpenseType>(),
				Currencies = EnumHelper.GetEnumSelectList<Currency>()
			};
			return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(MasterExpenseDTO model)
        {
			model.PersonnelId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

			try
			{
                if (!ModelState.IsValid)
                {
					ViewBag.EnumValues = new
					{
						ExpenseTypes = EnumHelper.GetEnumSelectList<ExpenseType>(),
						Currencies = EnumHelper.GetEnumSelectList<Currency>()
					};
					return View(model);
                }

				await _apiService.PostWithImageAsync<MasterExpenseDTO, MasterExpenseDTO>("Expense", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde oluşturuldu.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
        }


        public async Task<IActionResult> Update(string id)
        {
			ViewBag.EnumValues = new
			{
				ExpenseTypes = EnumHelper.GetEnumSelectList<ExpenseType>(),
				Currencies = EnumHelper.GetEnumSelectList<Currency>()
			};

			UpdateMasterExpenseDTO expense = await _apiService.GetByIdAsync<UpdateMasterExpenseDTO>("expense/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateMasterExpenseDTO model)
        {
			
			try
            {
                if (!ModelState.IsValid)
                {
					ViewBag.EnumValues = new
					{
						ExpenseTypes = EnumHelper.GetEnumSelectList<ExpenseType>(),
						Currencies = EnumHelper.GetEnumSelectList<Currency>()
					};
					return View(model);
                }
                await _apiService.PutWithImageAsync<UpdateMasterExpenseDTO,UpdateMasterExpenseDTO>("expense", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncelleme sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
        }

        //TODO: Sil çalışmıyor.
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<MasterExpenseVM>($"expense", id, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde silindi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt silme sırasında hata oluştu : {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}
