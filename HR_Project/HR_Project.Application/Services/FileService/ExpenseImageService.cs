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
using HR_Project.Domain.Enum;

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

		public async Task<bool> UploadFile(string expenselId, IFormFile file)
		{
			try
			{
				List<(string fileName, string pathOrContainerName)> result = await _azureStorage.UploadAsync("cost-files", file);

				Expense expense = await _expenseRepository.GetDefault(x => x.Id == Convert.ToInt32(expenselId));


				if (expense.ImageId!=null)
				{
					var pic = await _fileRepository.GetDefault(x => x.Id == expense.ImageId);
					pic.Status = Status.Deleted;
					pic.DeletedDate = DateTime.Now;

					await _fileRepository.Delete(pic);
				}
				
				CostPicture costPicture = new CostPicture
				{
					FilePath = result[0].pathOrContainerName,
					FileName = result[0].fileName,
					ExpenseId = expense.Id,
					Status = Status.Inserted,
					CreatedDate = DateTime.Now
				};

				expense.CostPicture = costPicture;

				await _fileRepository.Create(costPicture);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}
