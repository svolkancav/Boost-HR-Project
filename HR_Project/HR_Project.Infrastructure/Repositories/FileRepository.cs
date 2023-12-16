using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Domain.Repositories;
using HR_Project.Infrastructure.Context;
using F=HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Infrastructure.Repositories
{
	public class FileRepository : BaseRepository<F::File>, IFileRepository
	{
		public FileRepository(AppDbContext context) : base(context)
		{
		}

	}
}
