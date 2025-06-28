using RestX.WebApp.Models.ViewModels;

namespace RestX.WebApp.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> JsonToDishList(CartViewModel cart);
        Task<CartViewModel> JsonToCartViewModel(string cartJson);
        Task<UniversalValue<Guid>> CreatedOrder(CartViewModel model);
        Task<UniversalValue<Guid[]>> CreatedOrderDetails(CartViewModel model);
        Task<UniversalValue<Guid[]>> CreatedOrderDetails(DishCartViewModel[] model, Guid OrderId);
        Task<UniversalValue<Guid>> CreatedOrderDetail(DishCartViewModel model, Guid OrderId);

    }
}
