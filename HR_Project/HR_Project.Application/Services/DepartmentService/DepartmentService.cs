using HR_Project.Common.Models.DTOs;
using HR_Project.Domain.Enum;
using HR_Project.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentrepository;

        public DepartmentService(IDepartmentRepository departmentrepository)
        {
            _departmentrepository = departmentrepository;
        }

        public async Task<List<DepartmentDTO>> GetDepartments()
        {
            return await _departmentrepository.GetFilteredList(x => new DepartmentDTO
            {
                Id = x.Id,
                CompanyId = x.CompanyId,
                Description = x.Description,
                ManagerId = x.ManagerId,
                Name = x.Name,
            }, x => x.Status != Status.Deleted);
        }
    }
}
