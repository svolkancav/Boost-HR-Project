using HR_Project.Application.Services.AbsenceService;
using HR_Project.Application.Services.ExpenseService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HR_Project.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExpenseController : Controller
	{
		private readonly IExpenseService _expenseService;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ExpenseController(IExpenseService expenseService, IHttpContextAccessor httpContextAccessor)
		{
			_expenseService = expenseService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var expenses = await _expenseService.GetExpenses();
			return Ok(expenses);
		}

		[HttpGet]
		[Route("getbyid/{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var expense = await _expenseService.GetById(id);
			return Ok(expense);
		}

		[HttpGet]
		[Route("{condition}")]
		public async Task<IActionResult> GetByCondition(ConditionType condition)
		{
			var expenses = await _expenseService.GetByCondition(condition);
			return Ok(expenses);
		}

		[HttpPost]
		public async Task<IActionResult> Create()
		{
			var form = await Request.ReadFormAsync();
			var jsonString = form["model"]; // JSON verisi

			// JSON verisini RegisterDTO'ya çözme
			var model = JsonConvert.DeserializeObject<MasterExpenseDTO>(jsonString);

			// IFormFile'ı almak için
			var files = form.Files;

			for (int i = 0; i < model.Expenses.Count; i++)
			{
				var fieldName = $"UploadImage_{i}";
				var matchingFile = files.FirstOrDefault(f => f.Name == fieldName);

				if (matchingFile != null)
				{
					model.Expenses[i].UploadImage = matchingFile;
				}
			}

			await _expenseService.Create(model);
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Update(UpdateMasterExpenseDTO model)
		{
			await _expenseService.Update(model);
			return Ok();
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _expenseService.Delete(id);
			return Ok();
		}
	}
}
