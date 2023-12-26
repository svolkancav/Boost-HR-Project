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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
		private readonly IMasterExpenseRepository _masterExpenseRepository;
		private readonly IMapper _mapper;
		private readonly IExpenseImageService _expenseImageService;
		private readonly IConfiguration _configuration;

		public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper, IExpenseImageService expenseImageService, IMasterExpenseRepository masterExpenseRepository, IConfiguration configuration)
		{
			_expenseRepository = expenseRepository;
			_mapper = mapper;
			_expenseImageService = expenseImageService;
			_masterExpenseRepository = masterExpenseRepository;
			_configuration = configuration;
		}

		public async Task Create(MasterExpenseDTO model)
		{
			List<Expense> expenses = new List<Expense>();

			foreach (ExpenseDTO expenseDTO in model.Expenses)
			{
				Expense expense = _mapper.Map<Expense>(expenseDTO);
				expense.CreatedDate = DateTime.Now;
				expense.Status = Status.Inserted;

				expenses.Add(expense);

			}

			await _masterExpenseRepository.Create(new MasterExpense
			{
				CreatedDate = DateTime.Now,
				Status = Status.Inserted,
				Expenses = expenses,
				AggregateAmount = expenses.Sum(x => x.ExpenseAmount),
				Condition = ConditionType.Pending,
				PersonnelId = model.PersonnelId
			});

			for (int i = 0; i < model.Expenses.Count; i++)
			{
				if (model.Expenses[i].UploadImage != null)
				{
					await _expenseImageService.UploadFile(expenses[i].Id.ToString(), model.Expenses[i].UploadImage);
				}
			}

		}

		public async Task Delete(int id)
		{
			UpdateMasterExpenseDTO masterExpenseDTO = await _masterExpenseRepository.GetFilteredFirstOrDefault(x => new UpdateMasterExpenseDTO
			{
				Id = x.Id,
				AggregateAmount = x.AggregateAmount,
				CreateDate = x.CreatedDate,
				Expenses = _mapper.Map<List<ExpenseDTO>>(x.Expenses)
			}, x => x.Id == id, include: x => x.Include(x => x.Expenses));


			List<Expense> expenses = _mapper.Map<List<Expense>>(masterExpenseDTO.Expenses);

			MasterExpense masterExpense = await _masterExpenseRepository.GetDefault(x => x.Id == id);

			if (id == 0)
			{
				throw new ArgumentException("Id 0 Olamaz!");

			}
			else if (masterExpense == null)
			{
				throw new ArgumentException("Böyle bir masraf mevcut değil!");
			}

			masterExpense.DeletedDate = DateTime.Now;
			masterExpense.Status = Status.Deleted;
			await _masterExpenseRepository.Delete(masterExpense);
			foreach (Expense expense in expenses)
			{
				expense.Status = Status.Deleted;
				expense.DeletedDate = DateTime.Now;
				await _expenseImageService.DeleteFile(expense.Id.ToString());
			}
		}

		public async Task<List<MasterExpenseVM>> GetByCondition(ConditionType conditionType)
		{
			return await _masterExpenseRepository.GetFilteredList(x => new MasterExpenseVM
			{
				AggregateAmount = x.AggregateAmount,
				Condition = x.Condition,
				CreateDate = x.CreatedDate,
				Id = x.Id,
				Expenses = _mapper.Map<List<ExpenseDTO>>(x.Expenses)
			}, x => x.Condition == conditionType, include: x => x.Include(x => x.Expenses));
		}

		public async Task<UpdateMasterExpenseDTO> GetById(string id)
		{
			List<ExpenseDTO> expensesDTO = await _expenseRepository.GetFilteredList(x => new ExpenseDTO
			{
				ExpenseAmount = x.ExpenseAmount,
				ExpenseDate = x.ExpenseDate,
				ExpenseType = x.ExpenseType,
				Reason = x.Reason,
				Currency = x.Currency,
				ImageUrl = $"{_configuration["BaseStorageUrl"]}/{x.CostPicture.FilePath}"
			}, x => x.MasterExpenseId == Convert.ToInt32(id));

			UpdateMasterExpenseDTO masterExpenseDTO = await _masterExpenseRepository.GetFilteredFirstOrDefault(x => new UpdateMasterExpenseDTO
			{
				Id = x.Id,
				AggregateAmount = x.AggregateAmount,
				CreateDate = x.CreatedDate,
				Expenses = expensesDTO,
			}, x => x.Id == Convert.ToInt32(id));


			return masterExpenseDTO;
		}

		public async Task<List<MasterExpenseVM>> GetExpenses()
		{

			List<MasterExpenseVM> masterExpenses = await _masterExpenseRepository.GetFilteredList(x => new MasterExpenseVM
			{
				Id = x.Id,
				AggregateAmount = x.AggregateAmount,
				CreateDate = x.CreatedDate,
				Condition = x.Condition,
			}, x => x.Status != Status.Deleted
			);

			foreach (var item in masterExpenses)
			{
				List<ExpenseDTO> expensesDTO = await _expenseRepository.GetFilteredList(x => new ExpenseDTO
				{
					ExpenseAmount = x.ExpenseAmount,
					ExpenseDate = x.ExpenseDate,
					ExpenseType = x.ExpenseType,
					Reason = x.Reason,
					Currency = x.Currency,
					ImageUrl = String.IsNullOrEmpty(x.CostPicture.FilePath) ? null : $"{_configuration["BaseStorageUrl"]}/{x.CostPicture.FilePath}"
				}, x => x.MasterExpenseId == item.Id, include: x => x.Include(x => x.CostPicture));
				item.Expenses = expensesDTO;
			}



			return masterExpenses;
		}

		public async Task Update(UpdateMasterExpenseDTO model)
		{
			UpdateMasterExpenseDTO masterExpenseDTO = await _masterExpenseRepository.GetFilteredFirstOrDefault(x => new UpdateMasterExpenseDTO
			{
				Id = x.Id,
				AggregateAmount = x.AggregateAmount,
				CreateDate = x.CreatedDate,
				Expenses = _mapper.Map<List<ExpenseDTO>>(x.Expenses)
			}, x => x.Id == model.Id, include: x => x.Include(x => x.Expenses));

			List<Expense> expensesDTO = new List<Expense>();
			expensesDTO = _mapper.Map<List<Expense>>(model.Expenses);

			List<Expense> expenses = _mapper.Map<List<Expense>>(masterExpenseDTO.Expenses);

			MasterExpense masterExpense = await _masterExpenseRepository.GetDefault(x => x.Id == model.Id);

			masterExpense.Status = Status.Updated;
			masterExpense.ModifiedDate = DateTime.Now;
			masterExpense.Expenses = expensesDTO;
			masterExpense.AggregateAmount = model.Expenses.Sum(x => x.ExpenseAmount);


			await _masterExpenseRepository.Update(masterExpense);

			for (int i = 0; i < model.Expenses.Count; i++)
			{
				if (model.Expenses[i].UploadImage != null)
				{
					await _expenseImageService.UploadFile(expensesDTO[i].Id.ToString(), model.Expenses[i].UploadImage);
				}
				if (expenses.Count == model.Expenses.Count)
				{
					if (expenses[i].ImageId != null && model.Expenses[i].UploadImage == null)
					{
						await _expenseImageService.DeleteFile(expensesDTO[i].Id.ToString());
					}
				}
			}
		}
	}
}
