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
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;

        public DishManagementService(
            IRepository repo, 
            IDishService dishService, 
            ICategoryService categoryService, 
            IFileService fileService,
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _dishService = dishService;
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public async Task<DishesManagementViewModel> GetDishesManagementViewModelAsync(Guid ownerId)
        {
            var dishes = await _dishService.GetDishesByOwnerIdAsync(ownerId);
            var categories = await _categoryService.GetCategoriesAsync(); 

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

        public async Task<DishViewModel?> GetDishViewModelByIdAsync(int id)
        {
            var d = await _dishService.GetDishByIdAsync(id);
            if (d == null) return null;
            return new DishViewModel
            {
                Id = d.Id,
                CategoryId = d.CategoryId, 
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
            // ImageUrl is now file path, validation handled in controller
            Guid? fileId = null;

            if (!string.IsNullOrWhiteSpace(viewModel.ImageUrl))
            {
                string fileName = Path.GetFileName(viewModel.ImageUrl);

                if (viewModel.Id > 0)
                {
                    var existingDish = await _dishService.GetDishByIdAsync(viewModel.Id);
                    var file = await _fileService.CreateOrUpdateFileAsync(
                        existingDish?.FileId,
                        fileName,
                        viewModel.ImageUrl,
                        userId);
                    fileId = file?.Id;
                }
                else
                {
                    fileId = await _fileService.CreateFileAsync(fileName, viewModel.ImageUrl, userId);
                }
            }

            var entity = new Dish
            {
                Id = viewModel.Id,
                OwnerId = ownerId,
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                IsActive = viewModel.IsActive,
                FileId = fileId
            };

            return await _dishService.UpsertDishAsync(entity, userId);
        }

        public async Task DeleteDishAsync(int id)
        {
            // Get dish with file info before deletion
            var dish = await _dishService.GetDishByIdAsync(id);
            
            // Delete the dish
            await _dishService.DeleteDishAsync(id);
            
            // Delete associated file if exists
            if (dish?.FileId.HasValue == true)
            {
                await _fileService.DeleteFileAsync(dish.FileId.Value);
            }
        }
    }
}