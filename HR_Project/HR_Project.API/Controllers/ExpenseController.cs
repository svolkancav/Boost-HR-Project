using HR_Project.Application.Services.AbsenceService;
using HR_Project.Application.Services.ExpenseService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
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
        public async Task<IActionResult> Create(ExpenseDTO model)
        {
            await _expenseService.Create(model);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateExpenseDTO model)
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
