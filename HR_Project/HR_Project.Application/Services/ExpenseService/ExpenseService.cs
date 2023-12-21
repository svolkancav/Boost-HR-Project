using AutoMapper;
using HR_Project.Application.Services.FileService;
using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Entities.Concrete;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Personnel> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExpenseImageService _expenseImageService;

		public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper, UserManager<Personnel> userManager, IHttpContextAccessor httpContextAccessor, IExpenseImageService expenseImageService)
		{
			_expenseRepository = expenseRepository;
			_mapper = mapper;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
			_expenseImageService = expenseImageService;
		}

		public async Task Create(Expense_MasterExpenseVM model)
        {
            Personnel personnel = await _userManager.FindByEmailAsync(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value);

            Expense expense = _mapper.Map<Expense>(model);

            List<Expense> expenses = new List<Expense>();
            
            MasterExpense masterExpense = new MasterExpense();

            masterExpense.Status = Status.Inserted;
            masterExpense.CreatedDate = DateTime.Now;
            masterExpense.Expenses = new List<Expense>();

            

            expense.Status = Status.Inserted;
            expense.CreatedDate = DateTime.Now;
            expense.PersonnelId = personnel.Id;
            //try
            //{
            //    if(model.UploadImage != null)
            //        await _expenseImageService.UploadFile(expense.Id.ToString(), model.UploadImage);
            //    await _expenseRepository.Create(expense);

            //}
            //catch (Exception message)
            //{

            //    throw message;
            //}
        }

        public async Task Delete(int id)
        {
            Expense expense = await _expenseRepository.GetDefault(x => x.Id == id);
            if (id == 0)
            {
                throw new ArgumentException("Id 0 Olamaz!");

            }
            else if (expense == null)
            {
                throw new ArgumentException("Böyle bir masraf mevcut değil!");
            }

            expense.DeletedDate = DateTime.Now;
            expense.Status = Status.Deleted;
            await _expenseRepository.Delete(expense);
        }

        public async Task<List<ExpenseVM>> GetByCondition(ConditionType conditionType)
        {
            return await _expenseRepository.GetFilteredList(x => new ExpenseVM
            {
                Id = x.Id,
                ExpenseType=x.ExpenseType,
                ExpenseAmount=x.ExpenseAmount,
                ExpenseDate=x.ExpenseDate,
                Currency=x.Currency,
                Reason=x.Reason,
                Condition=x.Condition,
            }, x => x.Status != Status.Deleted && x.Condition == conditionType);
        }

        public async Task<UpdateExpenseDTO> GetById(string id)
        {
            UpdateExpenseDTO model = new UpdateExpenseDTO();

            Expense expense = await _expenseRepository.GetDefault(x => x.Id == Convert.ToInt32(id));


            var result = _mapper.Map<UpdateExpenseDTO>(expense);
            return result;
        }

        public async Task<List<ExpenseVM>> GetExpenses()
        {
            return await _expenseRepository.GetFilteredList(x => new ExpenseVM
            {
                Id = x.Id,
                ExpenseType = x.ExpenseType,
                ExpenseAmount = x.ExpenseAmount,
                ExpenseDate = x.ExpenseDate,
                Currency = x.Currency,
                Reason = x.Reason,
                Condition = x.Condition,
            }, x => x.Status != Status.Deleted);
        }

        public async Task Update(UpdateExpenseDTO model)
        {
            Expense expense =await _expenseRepository.GetDefault(x=>x.Id == model.Id);

            expense.ExpenseType = model.ExpenseType;
            expense.ExpenseDate = model.ExpenseDate;
            expense.ExpenseAmount = model.ExpenseAmount;
            expense.Currency = model.Currency;
            expense.Reason = model.Reason;
            expense.Condition = model.Condition;
            expense.ModifiedDate=DateTime.Now;
            expense.Status = Status.Updated;

            if (model.UploadImage != null)
				await _expenseImageService.UploadFile(expense.Id.ToString(), model.UploadImage);

            _expenseRepository.Update(expense);
        }
    }
}
