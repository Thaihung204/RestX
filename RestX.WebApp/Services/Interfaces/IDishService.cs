using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDishService
    {
        Task<List<Dish>> GetDishesByOwnerIdAsync();
        Task<List<Dish>> GetAllDishesByOwnerIdAsync(); // New method to get all dishes including inactive
        Task<Dish> GetDishByIdAsync(int id);
        Task<DishViewModel> GetDishViewModelByIdAsync(int id);
        Task<int> UpsertDishAsync(DataTransferObjects.Dish request);
        Task DeleteDishAsync(int id);
        Task<bool> UpdateDishAvailabilityAsync(int dishId, bool isActive); // New method for updating availability
    }
}