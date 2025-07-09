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
        private readonly IDishService dishService;
        private readonly ICategoryService categoryService;

        public DishManagementService(
            IRepository repo, 
            IDishService dishService, 
            ICategoryService categoryService, 
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            this.dishService = dishService;
            this.categoryService = categoryService;
        }

        public async Task<DishesManagementViewModel> GetDishesManagementViewModelAsync(Guid ownerId)
        {
            var dishes = await dishService.GetDishesByOwnerIdAsync(ownerId);
            var categories = await categoryService.GetCategoriesAsync(); 

            return new DishesManagementViewModel
            {
                Dishes = dishes.Select(d => new DishViewModel
                {
                    Id = d.Id,
                    CategoryId = d.CategoryId, 
                    Name = d.Name,
                    Description = d.Description,
                    Price = d.Price,
                    ImageUrl = d.File?.Url,
                    CategoryName = d.Category?.Name ?? string.Empty,
                    IsActive = d.IsActive
                }).ToList(),
                Categories = categories 
            };
        }
    }
}