using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
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

                List<Expense_MasterExpenseVM> expense_MasterExpenses = await _apiService.GetAsync<List<Expense_MasterExpenseVM>>("masterExpense", HttpContext.Request.Cookies["access-token"]);
                List<Expense_MasterExpenseVM> selectedExpense_MasterExpenses = expense_MasterExpenses.Where(x => x.Reason.ToLower().Contains(searchText.ToLower()) || x.ExpenseAmount.ToString().Contains(searchText)).ToList();

                return View(selectedExpense_MasterExpenses.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                List<Expense_MasterExpenseVM> expense_MasterExpenses = await _apiService.GetAsync<List<Expense_MasterExpenseVM>>("masterExpense", HttpContext.Request.Cookies["access-token"]);
                return View(expense_MasterExpenses.ToPagedList(pageNumber, pageSize));
            }

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense_MasterExpenseVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _apiService.PostAsync<Expense_MasterExpenseVM, Expense_MasterExpenseVM>("masterExpense", model, HttpContext.Request.Cookies["access-token"]);
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
            UpdateExpenseDTO expense = await _apiService.GetByIdAsync<UpdateExpenseDTO>("expense/getbyid", id, HttpContext.Request.Cookies["access-token"]);
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateExpenseDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _apiService.UpdateAsync<UpdateExpenseDTO>("expense", model, HttpContext.Request.Cookies["access-token"]);
                Toastr("success", "Kayıt başarılı bir şekilde güncellendi.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Toastr("error", $"Kayıt güncelleme sırasında hata oluştu : {ex.Message}");
                return View(model);
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _apiService.DeleteAsync<ExpenseDTO>($"expense", id, HttpContext.Request.Cookies["access-token"]);
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
