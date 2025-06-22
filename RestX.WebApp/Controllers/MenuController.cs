using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IActionResult> Index(Guid ownerID, CancellationToken cancellationToken)
        {
            var model = await _menuService.GetMenuViewModelAsync(ownerID, cancellationToken);
            return View(model);
        }
    }
}
