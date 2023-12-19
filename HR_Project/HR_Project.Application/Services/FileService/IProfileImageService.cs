using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Application.Services.FileService
{
	public interface IProfileImageService : IFileService<PersonnelPicture>
	{
	}
}
