using RestX.WebApp.Models.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestX.WebApp.Services.Interfaces
{
    public interface IDishManagementService
    {
        Task<DishesManagementViewModel> GetDishesManagementViewModelAsync(Guid ownerId);
        Task<DishViewModel?> GetDishViewModelByIdAsync(int id);
        Task<int> UpsertDishAsync(DishViewModel viewModel, Guid ownerId, string userId);
        Task DeleteDishAsync(int id);
    }
}
