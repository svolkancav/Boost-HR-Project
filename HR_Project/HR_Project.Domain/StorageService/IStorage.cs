using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Domain.Services.StorageService
{
	public interface IStorage
	{
		Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFile file);
		Task DeleteAsync(string pathOrContainerName, string fileName);
		List<string> GetFiles(string pathOrContainerName);
		bool HasFile(string pathOrContainerName, string fileName);
	}
}
