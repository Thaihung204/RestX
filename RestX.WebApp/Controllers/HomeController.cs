using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService, IExceptionHandler exceptionHandler) : base(exceptionHandler) 
        {
            this.homeService = homeService;
        }

        [HttpGet]
        [Route("Home/Index/{ownerId:guid}/{tableId:int}")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                var viewModel = await homeService.GetHomeViewsAsync(cancellationToken);

                if (viewModel == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing Index for OwnerId");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
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
