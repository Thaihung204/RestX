using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartService cartService;
        public CartController(IExceptionHandler exceptionHandler, ICartService cartService) : base(exceptionHandler)
        {
            this.cartService = cartService;
        }

        [HttpGet]
        [Route("Cart/Index/{ownerId:guid}/{tableId:int}")]
        public async Task<IActionResult> Index(CartViewModel model)
        {
            model = await cartService.JsonToDishList(model);
            return View(model);
        }
    }
}
