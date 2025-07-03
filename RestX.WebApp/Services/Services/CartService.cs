using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Text.Json;

namespace RestX.WebApp.Services.Services
{
    public class CartService : BaseService, ICartService
    {

        public CartService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<CartViewModel> JsonToDishList(CartViewModel cart)
        {
            cart.DishList = JsonSerializer.Deserialize<DishCartViewModel[]>(cart.DishListJson);
            return cart;
        }

    }
}
