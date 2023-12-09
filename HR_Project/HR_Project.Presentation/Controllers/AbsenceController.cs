using HR_Project.Presentation.APIService;
using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.Controllers
{
    public class AbsenceController : Controller
    {
        private readonly IAPIService _apiService;

        public AbsenceController(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
