using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Controllers
{
    [Route("Owner")]
    public class OwnerController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly IDishManagementService dishManagementService;
        private readonly IDishService dishService;
        private readonly ICategoryService categoryService;

        public OwnerController(
            IDashboardService dashboardService,
            IDishManagementService dishManagementService,
            IDishService dishService,
            ICategoryService categoryService,
            IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.dashboardService = dashboardService;
            this.dishManagementService = dishManagementService;
            this.categoryService = categoryService;
            this.dishService = dishService;
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
        [Route("DashBoard")]
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

        [HttpGet("Dishes")]
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

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await categoryService.GetCategoriesAsync();
                var data = categories.Select(c => new { id = c.Id, name = c.Name }).ToList();
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading categories.");
                return Json(new { success = false, message = "Failed to load categories." });
            }
        }

        [HttpPost("Dishes/Upsert")]
        [HttpPost("Dishes/Create")]   
        [HttpPost("Dishes/Edit/{id:int?}")]   
        public async Task<IActionResult> UpsertDish([FromForm] DishRequest request, int? id = null)
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();

                bool isEdit = (id.HasValue && id.Value > 0) || (request.Id.HasValue && request.Id.Value > 0);
                int? dishId = id ?? request.Id;

                if (isEdit && ModelState.ContainsKey("ImageFile"))
                {
                    ModelState.Remove("ImageFile");
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, errors });
                }

                var resultDishId = await dishService.UpsertDishAsync(request, ownerId, dishId);

                if (resultDishId == null)
                {
                    string errorMessage = isEdit ? "Dish not found." : "Owner not found.";
                    return Json(new { success = false, message = errorMessage });
                }

                string successMessage = isEdit ? "Dish updated successfully!" : "Dish created successfully!";
                return Json(new { success = true, message = successMessage, dishId = resultDishId });
            }
            catch (Exception ex)
            {
                string operation = (id.HasValue && id.Value > 0) ? "updating" : "creating";
                this.exceptionHandler.RaiseException(ex, $"An error occurred while {operation} the dish.");
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("Dishes/Delete/{id:int}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                await dishService.DeleteDishAsync(id);
                return Json(new { success = true, message = "Dish deleted successfully!" });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while deleting the dish.");
                return Json(new { success = false, message = "An error occurred while deleting the dish." });
            }
        }

        [HttpGet("Dishes/Detail/{id:int}")]
        public async Task<IActionResult> DishDetail(int id)
        {
            try
            {
                var dishViewModel = await dishService.GetDishViewModelByIdAsync(id);
                if (dishViewModel == null)
                    return Json(new { success = false, message = "Dish not found." });

                return Json(new { success = true, data = dishViewModel });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading dish detail.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}