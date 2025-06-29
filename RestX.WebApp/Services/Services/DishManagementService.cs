using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Services
{
    public class DishManagementService : BaseService, IDishManagementService
    {
        private readonly IDishService _dishService;

        public DishManagementService(IRepository repo, IDishService dishService, IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _dishService = dishService;
        }

        public async Task<DishesManagementViewModel> GetDishesManagementViewModelAsync(Guid ownerId)
        {
            var dishes = await _dishService.GetDishesByOwnerIdAsync(ownerId);
            return new DishesManagementViewModel
            {
                OwnerId = ownerId,
                Dishes = dishes.Select(d => new DishViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    ImageUrl = d.File?.Url,
                    CategoryName = d.Category?.Name ?? string.Empty,
                    IsActive = d.IsActive
                }).ToList()
            };
        }

        public async Task<DishViewModel?> GetDishViewModelByIdAsync(int id)
        {
            var d = await _dishService.GetDishByIdAsync(id);
            if (d == null) return null;
            return new DishViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                ImageUrl = d.File?.Url,
                CategoryName = d.Category?.Name ?? string.Empty,
                IsActive = d.IsActive
            };
        }

        public async Task<int> UpsertDishAsync(DishViewModel viewModel, Guid ownerId, string userId)
        {
            var entity = new Dish
            {
                Id = viewModel.Id,
                OwnerId = ownerId,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                IsActive = viewModel.IsActive
            };
            return await _dishService.UpsertDishAsync(entity, userId);
        }

        public async Task DeleteDishAsync(int id)
        {
            await _dishService.DeleteDishAsync(id);
        }
    }
}