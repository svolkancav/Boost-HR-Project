using HR_Project.Application.Services.DepartmentService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

    }
}
