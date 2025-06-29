using RestX.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDishService
    {
        Task<List<Dish>> GetDishesByOwnerIdAsync(Guid ownerId);
        Task<Dish?> GetDishByIdAsync(int id);
        Task<int> UpsertDishAsync(Dish entity, string userId);
        Task DeleteDishAsync(int id);
    }
}