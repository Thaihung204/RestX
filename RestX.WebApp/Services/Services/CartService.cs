using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace RestX.WebApp.Services.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CartService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CartViewModel> JsonToDishList(CartViewModel cart)
        {
            cart.DishList = JsonSerializer.Deserialize<DishCartViewModel[]>(cart.DishListJson);
            return cart;
        }

        public async Task<UniversalValue<Guid>> CreatedOrder(CartViewModel model)
        {
            DateTime currentTime = DateTime.Now;
            string customerId = httpContextAccessor.HttpContext.Session.GetString("CustomerId");
            Order newOrder = null;
            string message = null;
            try
            {
                newOrder = new Order()
                {
                    CustomerId = Guid.Parse(customerId),
                    TableId = model.TableId,
                    OwnerId = model.OwnerId,
                    OrderStatusId = 1,
                    Time = currentTime,
                    IsActive = true,
                };
            } catch (ArgumentNullException ane)
            {
                return UniversalValue<Guid>.Failure("Bạn hãy vui lòng đăng nhập!");
            }

            Guid temp = Guid.Parse((await Repo.CreateAsync(newOrder)).ToString());
            await Repo.SaveAsync();

            return UniversalValue<Guid>.Success(temp, "Tạo Order thành công");
        }

        public async Task<UniversalValue<Guid[]>> CreatedOrderDetails(CartViewModel model)
        {
            model = await JsonToDishList(model);
            return await CreatedOrderDetails(model.DishList, model.OrderId.Value);
        }

        public async Task<UniversalValue<Guid[]>> CreatedOrderDetails(DishCartViewModel[] modelList, Guid OrderId)
        {
            Guid[] orderDetailIdList = new Guid[modelList.Length];
            int i = 0;

            try
            {
                foreach (DishCartViewModel model in modelList)
                {
                    UniversalValue<Guid> returnGuid = await CreatedOrderDetail(model, OrderId);

                    if (!returnGuid.ErrorMessage.IsNullOrEmpty())
                    {
                        return UniversalValue<Guid[]>.Failure(returnGuid.ErrorMessage);
                    }

                    orderDetailIdList[i] = returnGuid.Data;
                    i++;
                }
            } catch (Exception ex)
            {
                return UniversalValue<Guid[]>.Failure("Ối! Có gì đó không ổn ở " + nameof(CreatedOrderDetails));
            }

            return UniversalValue<Guid[]>.Success(orderDetailIdList, null);
        }

        public async Task<UniversalValue<Guid>> CreatedOrderDetail(DishCartViewModel model, Guid OrderId)
        {
            DateTime currentTime = DateTime.Now;
            string customerId = httpContextAccessor.HttpContext.Session.GetString("CustomerId");
            OrderDetail newOrderDetail = null;
            Guid temp = Guid.Empty;
            try
            {
                newOrderDetail = new OrderDetail()
                {
                    OrderId = OrderId,
                    DishId = model.DishId,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    IsActive = true,
                };
                temp = Guid.Parse((await Repo.CreateAsync(newOrderDetail, customerId)).ToString());
                await Repo.SaveAsync();
            } catch (Exception ex)
            {
                return UniversalValue<Guid>.Failure("Ối! Có gì đó không ổn ở " + nameof(CreatedOrderDetails));
            }
            

            return UniversalValue<Guid>.Success(temp, null);
        }

        public async Task<CartViewModel> JsonToCartViewModel(string cartJson)
        {
            CartViewModel cart = JsonSerializer.Deserialize<CartViewModel>(cartJson);
            return cart;
        }
    }
}
