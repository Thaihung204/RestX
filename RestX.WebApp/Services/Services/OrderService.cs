using Microsoft.IdentityModel.Tokens;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICartService cartService;
        public OrderService(IRepository repo, IHttpContextAccessor httpContextAccessor, ICartService cartService) : base(repo, httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cartService = cartService;
        }

        public async Task<UniversalValue<Guid>> CreatedOrderAndOrderDetails(CartViewModel model)
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
            }
            catch (ArgumentNullException ane)
            {
                return UniversalValue<Guid>.Failure("Bạn hãy vui lòng đăng nhập!");
            }

            Guid temp = Guid.Parse((await Repo.CreateAsync(newOrder)).ToString());
            await Repo.SaveAsync();

            model.OrderId = temp;

            UniversalValue<Guid[]> temp2 = await CreatedOrderDetails(model);
            if (!temp2.ErrorMessage.IsNullOrEmpty())
            {
                return UniversalValue<Guid>.Success(temp, "Tạo Order thành công, Tạo Order Detail thất bại");
            }

            return UniversalValue<Guid>.Success(temp, "Tạo Order thành công, Tạo Order Detail thành công");
        }

        public async Task<UniversalValue<Guid[]>> CreatedOrderDetails(CartViewModel model)
        {
            model = await cartService.JsonToDishList(model);
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
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
            {
                return UniversalValue<Guid>.Failure("Ối! Có gì đó không ổn ở " + nameof(CreatedOrderDetails));
            }


            return UniversalValue<Guid>.Success(temp, null);
        }
    }
}
