using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDishService
    {
        Task<List<Dish>> GetDishesByOwnerIdAsync(Guid ownerId);
        Task<Dish?> GetDishByIdAsync(int id);
        Task<DishViewModel?> GetDishViewModelByIdAsync(int id); // Thêm method này

        Task<int?> UpsertDishAsync(DishRequest entity, Guid ownerId, int? id = null);
        Task DeleteDishAsync(int id);
    }
}