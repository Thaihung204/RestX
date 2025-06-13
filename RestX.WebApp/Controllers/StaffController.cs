using Microsoft.AspNetCore.Mvc;

namespace RestX.WebApp.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
