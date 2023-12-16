using HR_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Domain.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
        
    }
}
