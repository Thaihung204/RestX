using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Text.Json;

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
            var tempModel = TempData["tempModel"];
            if (tempModel != null)
            {
                model = await cartService.JsonToCartViewModel(tempModel.ToString());
            }
                

            model = await cartService.JsonToDishList(model);

            if (model.Message != null) 
                ViewBag.Message = model.Message;

            return View(model);
        }

        [HttpPost]
        [Route("Cart/IndexPost/{ownerId:guid}/{tableId:int}")]
        public async Task<IActionResult> IndexPost(CartViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Message = "Ối! Có gì đó không ổn";
                TempData["tempModel"] = JsonSerializer.Serialize(model);
                return RedirectToAction("Index", new {OwnerId = model.OwnerId, TableId = model.TableId});
            }

            UniversalValue<Guid> returnUVOrderId = await cartService.CreatedOrder(model);
            if (!returnUVOrderId.ErrorMessage.IsNullOrEmpty())
            {
                model.Message = returnUVOrderId.ErrorMessage;
                TempData["tempModel"] = JsonSerializer.Serialize(model);
                return RedirectToAction("Index", new { OwnerId = model.OwnerId, TableId = model.TableId });
            }
                
            model.OrderId = returnUVOrderId.Data;
            UniversalValue<Guid[]> returnUVOrderDetailId = await cartService.CreatedOrderDetails(model);

            if (!returnUVOrderDetailId.ErrorMessage.IsNullOrEmpty())
                model.Message = returnUVOrderDetailId.ErrorMessage;

            TempData["tempModel"] = JsonSerializer.Serialize(model);
            return RedirectToAction("Index", new { OwnerId = model.OwnerId, TableId = model.TableId });
        }
    }
}
