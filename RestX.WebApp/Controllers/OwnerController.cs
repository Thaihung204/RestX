using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Controllers
{
    
    public class OwnerController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly IDishManagementService dishManagementService;
        private readonly IDishService dishService;
        private readonly ICategoryService categoryService;
        private readonly IFileService fileService;
        private readonly IOwnerService ownerService;
        private readonly IMapper mapper;

        public OwnerController(
            IDashboardService dashboardService,
            IDishManagementService dishManagementService,
            IDishService dishService,
            ICategoryService categoryService,
            IFileService fileService,
            IOwnerService ownerService,
            IMapper mapper,
            IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.dashboardService = dashboardService;
            this.dishManagementService = dishManagementService;
            this.categoryService = categoryService;
            this.dishService = dishService;
            this.fileService = fileService;
            this.ownerService = ownerService;
            this.mapper = mapper;
        }

        private Guid GetOwnerIdFromClaim()
        {
            var ownerIdClaim = User.FindFirst("OwnerId");
            if (ownerIdClaim == null || !Guid.TryParse(ownerIdClaim.Value, out var ownerId))
            {
                throw new UnauthorizedAccessException("OwnerId claim is missing or invalid.");
            }
            return ownerId;
        }

        [HttpGet]
        [Route("Owner/DashBoard")]
        public async Task<IActionResult> DashBoard(CancellationToken cancellationToken)
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();
                var model = await dashboardService.GetDashboardViewModelAsync(ownerId, cancellationToken);
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading the dashboard.");
                return View("Error");
            }
        }

        [HttpGet("Owner/Dishes")]
        public async Task<IActionResult> DishesManagement(CancellationToken cancellationToken)
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();
                var model = await dishManagementService.GetDishesManagementViewModelAsync(ownerId);
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading dishes management.");
                return View("Error");
            }
        }

        [HttpGet("Owner/Categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await categoryService.GetCategoriesAsync();
                return Json(new { success = true, data = categories });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading categories.");
                return Json(new { success = false, message = "Failed to load categories." });
            }
        }

        [HttpPost("Owner/Categories/Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Json(new { success = false, message = "Category name is required." });
                }

                var categoryId = await categoryService.CreateCategoryAsync(request.Name, User.Identity?.Name ?? "system");
                var category = await categoryService.GetCategoriesAsync();
                var newCategory = category.FirstOrDefault(c => c.Id == categoryId);

                return Json(new { 
                    success = true, 
                    message = "Category created successfully!", 
                    data = newCategory 
                });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while creating the category.");
                return Json(new { success = false, message = "An error occurred while creating the category." });
            }
        }

        [HttpPost("Owner/Dishes/Create")]
        public async Task<IActionResult> CreateDish([FromForm] CreateDishRequest request)
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();
                var userId = User.Identity?.Name ?? "system";

                // Validate request using model state
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, errors = errors });
                }

                // Get owner information for file naming
                var owner = await ownerService.GetOwnerByIdAsync(ownerId);
                if (owner == null)
                {
                    return Json(new { success = false, message = "Owner not found." });
                }

                // 1. Upload image and create File entity
                var imageUrl = await fileService.UploadDishImageAsync(request.ImageFile, owner.Name, request.Name);
                var fileEntity = await fileService.CreateFileFromUploadAsync(imageUrl, request.ImageFile.FileName, userId);

                // 2. Map DTO to Dish entity using AutoMapper
                var dishEntity = mapper.Map<Dish>(request);

                // 3. Set additional properties not handled by AutoMapper
                dishEntity.OwnerId = ownerId;
                dishEntity.FileId = fileEntity.Id;

                // 4. Create dish using IDishService
                var dishId = await dishService.UpsertDishAsync(dishEntity, userId);

                return Json(new { success = true, message = "Dish created successfully!", dishId = dishId });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while creating the dish.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("Owner/Dishes/GetDish/{id:int}")]
        public async Task<IActionResult> GetDish(int id)
        {
            try
            {
                var model = await dishManagementService.GetDishViewModelByIdAsync(id);
                if (model == null)
                {
                    return Json(new { success = false, message = "Dish not found." });
                }

                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading dish data.");
                return Json(new { success = false, message = "Failed to load dish data." });
            }
        }

        [HttpPost("Owner/Dishes/Edit/{id:int}")]
        public async Task<IActionResult> EditDish(int id, [FromBody] DishViewModel model)
        {
            try
            {
                model.Id = id;
                var ownerId = GetOwnerIdFromClaim();

                if (ModelState.IsValid)
                {
                    await dishManagementService.UpsertDishAsync(model, ownerId, User.Identity?.Name ?? "system");
                    return Json(new { success = true, message = "Dish updated successfully!" });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return Json(new { success = false, errors = errors });
            }
            catch (UnauthorizedAccessException)
            {
                return Json(new { success = false, message = "You are not authorized to perform this action." });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while updating the dish.");
                return Json(new { success = false, message = "An error occurred while updating the dish." });
            }
        }

        [HttpPost("Owner/Dishes/Delete/{id:int}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                await dishManagementService.DeleteDishAsync(id);
                return Json(new { success = true, message = "Dish deleted successfully!" });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while deleting the dish.");
                return Json(new { success = false, message = "An error occurred while deleting the dish." });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}