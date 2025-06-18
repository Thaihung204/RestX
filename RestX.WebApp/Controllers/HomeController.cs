using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICustomerService customerService;
        private readonly IOwnerService ownerService;

        public HomeController(ILogger<HomeController> logger, ICustomerService customerService, IOwnerService ownerService)
        {
            _logger = logger;
            this.customerService = customerService;
            this.ownerService = ownerService;
        }

        [HttpGet]
        [Route("Home/Index/{ownerId}/{tableId}")]
        public async Task<IActionResult> Index(Guid ownerId, int tableId)
        {
            _logger.LogInformation("Index action called with OwnerId: {OwnerId}, TableId: {TableId}", ownerId, tableId);

            Owner owner = await ownerService.GetOwnerByIdAsync(ownerId);

            var viewModel = new Home_GetOwnerViewModel
            {
                Id = owner.Id,
                FileId = owner.FileId,
                Name = owner.Name,
                Address = owner.Address,
                FileName = owner.File?.Name,
                FileUrl = owner.File?.Url
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
