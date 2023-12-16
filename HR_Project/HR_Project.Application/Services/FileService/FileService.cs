using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Concrete.FileEntities;
using HR_Project.Domain.Repositories;
using HR_Project.Domain.Services.StorageService;
using Microsoft.AspNetCore.Http;
using F=HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Application.Services.FileService
{
	public class FileService : IFileService
	{
		private readonly IStorage _azureStorage;
		private readonly IFileRepository _fileRepository;

		public FileService(IStorage azureStorage, IFileRepository fileRepository)
		{
			_azureStorage = azureStorage;
			_fileRepository = fileRepository;
		}

		public async Task<bool> DeleteFile(string ContainerName, string fileName)
		{
			try
			{
				await _azureStorage.DeleteAsync(ContainerName, fileName);
				F::File file = await _fileRepository.GetDefault(x => x.FileName == fileName);
				await _fileRepository.Delete(file);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<string> GetFileById(string ContainerName, int id)
		{
			throw new NotImplementedException();

		}

		public Task<bool> UploadFile(string ContainerName, IFormFile file)
		{
			throw new NotImplementedException();
		}
	}
}
