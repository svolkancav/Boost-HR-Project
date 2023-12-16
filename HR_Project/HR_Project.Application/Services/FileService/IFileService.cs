using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Application.Services.FileService
{
	public interface IFileService
	{
		Task<bool> UploadFile(string ContainerName, IFormFile file);
		Task<bool> DeleteFile(string ContainerName, string fileName);
		Task<string> GetFileById(string ContainerName,int id);
	}
}
