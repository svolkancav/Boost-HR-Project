using Microsoft.AspNetCore.Mvc;

namespace HR_Project.Presentation.Controllers
{
    public class BaseController : Controller
    {
        protected void Toastr(string type, string message)
        {
            TempData[$"Toastr{type}"] = message;
        }
    }
}
