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
        Task<DishViewModel?> GetDishViewModelByIdAsync(int id);
        Task<int?> UpsertDishAsync(DishRequest request, Guid ownerId);
        Task DeleteDishAsync(int id);
    }
}