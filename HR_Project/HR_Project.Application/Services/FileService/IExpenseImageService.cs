using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Concrete;

namespace HR_Project.Application.Services.FileService
{
	public interface IExpenseImageService : IFileService<Expense>
	{
	}
}
