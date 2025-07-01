using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;

namespace RestX.WebApp.Services.Services
{
    public class DishService : BaseService, IDishService
    {
        public DishService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor)
        {
        }

        public async Task<List<Dish>> GetDishesAsync()
        {
            var dishes = await Repo.GetAllAsync<Dish>();
            return dishes.ToList();
        }
    }
}
