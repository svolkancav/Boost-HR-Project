using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Entities.Abstract;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Context;
using F=HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Infrastructure.Repositories
{
	public class FileRepository<T>: BaseRepository<T>, IFileRepository<T> where T : class, IBaseEntity, new()
	{
		public FileRepository(AppDbContext context) : base(context)
		{
		}

	}
}
