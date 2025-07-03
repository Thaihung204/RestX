using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Services
{
    public class DishService : BaseService, IDishService
    {
        public DishService(IRepository repo, IHttpContextAccessor httpContextAccessor) : base(repo, httpContextAccessor) { }

        public async Task<List<Dish>> GetDishesByOwnerIdAsync(Guid ownerId)
        {
            var dishes = await repo.GetAsync<Dish>(
                filter: d => d.OwnerId == ownerId && d.IsActive == true,
                includeProperties: "Category,File"
            );
            return dishes.ToList();
        }

        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            var dishes = await repo.GetAsync<Dish>(
                filter: d => d.Id == id,
                includeProperties: "Category,File"
            );
            return dishes.FirstOrDefault();
        }

        public async Task<int> UpsertDishAsync(Dish entity, string userId)
        {
            if (entity.Id == 0)
            {
                var result = await repo.CreateAsync(entity, userId);
                await repo.SaveAsync();
                return (int)result;
            }
            else
            {
                repo.Update(entity, userId);
                await repo.SaveAsync();
                return entity.Id;
            }
        }

        public async Task DeleteDishAsync(int id)
        {
            repo.Delete<Dish>(id);
            await repo.SaveAsync();
        }
    }
}