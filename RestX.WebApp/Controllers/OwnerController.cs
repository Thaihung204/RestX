using Microsoft.AspNetCore.Mvc;

namespace RestX.WebApp.Controllers
{
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
