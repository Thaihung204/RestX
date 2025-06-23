using Microsoft.AspNetCore.Mvc;

namespace RestX.WebApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
