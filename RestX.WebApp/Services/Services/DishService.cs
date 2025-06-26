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
            var dishes = await repo.GetAllAsync<Dish>();
            return dishes.ToList();
        }
        public async Task<List<Dish>> GetDishesByOwnerIdAsync(Guid ownerId)
        {
            var dishes = await repo.GetAsync<Dish>(
                d => d.OwnerId == ownerId && d.IsActive == true
            );
            return dishes.ToList();
        }
    }
}
