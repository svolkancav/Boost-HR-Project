using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F=HR_Project.Domain.Entities.Concrete.FileEntities;

namespace HR_Project.Domain.Repositories
{
	public interface IFileRepository : IBaseRepository<F::File>
	{
	}
}
