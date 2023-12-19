using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace HR_Project.Application.Services.FileService
{
	public interface IFileService<T> where T : class, IBaseEntity, new()
	{
		Task<bool> UploadFile(string id, IFormFile file);
		Task<bool> DeleteFile(string id);
		Task<string> GetFileById(string id);
	}
}
