using HR_Project.Common.Models.DTOs;
using HR_Project.Common.Models.VMs;
using HR_Project.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Application.Services.DepartmentService
{
    public interface IDepartmentService
    {
        //Task Create(DepartmentDTO model);
        //Task Update(DepartmentDTO model);
        //Task Delete(int id);
        //Task<DepartmentDTO> GetById(string id);
        Task<List<DepartmentDTO>> GetDepartments();
    }
}
