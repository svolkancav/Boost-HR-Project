using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using HR_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using HR_Project.Domain.Repositories;
using HR_Project.Domain.Services.StorageService;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HR_Project.Application.Services.FileService
{
	public class ExpenseImageService : IExpenseImageService
	{
		private readonly IExpenseRepository _expenseRepository;
		private readonly IStorage _azureStorage;
		private readonly IFileRepository<CostPicture> _fileRepository;
		private readonly IConfiguration _configuration;

		public ExpenseImageService(IExpenseRepository expenseRepository, IStorage azureStorage, IFileRepository<CostPicture> fileRepository, IConfiguration configuration)
		{
			_expenseRepository = expenseRepository;
			_azureStorage = azureStorage;
			_fileRepository = fileRepository;
			_configuration = configuration;
		}

		public async Task<bool> DeleteFile(string expenselId)
		{
			try
			{
				Expense expense = await _expenseRepository.GetFilteredFirstOrDefault(x => new Expense(),
					   x => x.Id == Convert.ToInt32(expenselId),
					   include: x => x.Include(x => x.CostPicture));

				await _fileRepository.Delete(expense.CostPicture);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<string> GetFileById(string expenselId)
		{
			Expense expense = await _expenseRepository.GetFilteredFirstOrDefault(x => new Expense(),
					   x => x.Id == Convert.ToInt32(expenselId),
					   include: x => x.Include(x => x.CostPicture));

			return $"{_configuration["BaseStorageUrl"]}/{expense.CostPicture.FilePath}";

		}

		public async Task<bool> UploadFile(string expenselId, IFormFileCollection file)
		{
			try
			{
				List<(string fileName, string pathOrContainerName)> result = await _azureStorage.UploadAsync("cost-files", file);

				Expense expense = await _expenseRepository.GetFilteredFirstOrDefault(x => new Expense(),
						   x => x.Id == Convert.ToInt32(expenselId));

				
					expense.CostPicture = new CostPicture()
					{
						FileName = result[0].fileName,
						FilePath = result[0].pathOrContainerName
					};
				

				await _expenseRepository.Update(expense);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}
