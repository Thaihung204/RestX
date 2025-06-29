using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RestX.WebApp.Controllers
{
    public class OwnerController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly IDishManagementService dishManagementService;
        private readonly IDishService dishService;

        public OwnerController(IDashboardService dashboardService, IDishManagementService dishManagementService, IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.dashboardService = dashboardService;
            this.dishManagementService = dishManagementService;
        }

        [HttpGet]
        [Route("Owner/DashBoard/{ownerId:guid}")]
        public async Task<IActionResult> DashBoard(Guid ownerId, CancellationToken cancellationToken)
        {
            var model = await dashboardService.GetDashboardViewModelAsync(ownerId, cancellationToken);
            return View(model);
        }

        [HttpGet("Owner/Dishes/{ownerId:guid}")]
        public async Task<IActionResult> DishesManagement(Guid ownerId, CancellationToken cancellationToken)
        {
            var model = await dishManagementService.GetDishesManagementViewModelAsync(ownerId);
            return View(model);
        }

        [HttpGet("Owner/Dishes/Create/{ownerId:guid}")]
        public IActionResult CreateDish(Guid ownerId)
        {
            var model = new DishViewModel();
            return View(model);
        }

        [HttpPost("Owner/Dishes/Create/{ownerId:guid}")]
        public async Task<IActionResult> CreateDish(Guid ownerId, DishViewModel model)
        {
            if (ModelState.IsValid)
            {
                await dishManagementService.UpsertDishAsync(model, ownerId, User.Identity?.Name ?? "system");
                return RedirectToAction("DishesManagement", new { ownerId });
            }
            return View(model);
        }

        [HttpGet("Owner/Dishes/Edit/{id:int}/{ownerId:guid}")]
        public async Task<IActionResult> EditDish(int id, Guid ownerId)
        {
            var model = await dishManagementService.GetDishViewModelByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost("Owner/Dishes/Edit/{id:int}/{ownerId:guid}")]
        public async Task<IActionResult> EditDish(int id, Guid ownerId, DishViewModel model)
        {
            if (ModelState.IsValid)
            {
                await dishManagementService.UpsertDishAsync(model, ownerId, User.Identity?.Name ?? "system");
                return RedirectToAction("DishesManagement", new { ownerId });
            }
            return View(model);
        }

        [HttpGet("Owner/Dishes/DetailDish/{id:int}/{ownerId:guid}")]
        public async Task<IActionResult> DetailDish(int id, Guid ownerId)
        {
            var model = await dishManagementService.GetDishViewModelByIdAsync(id);
            if (model == null) return NotFound();
            return View("Dishes/DetailDish", model);
        }

        [HttpPost("Owner/Dishes/Delete/{id:int}/{ownerId:guid}")]
        public async Task<IActionResult> DeleteDish(int id, Guid ownerId)
        {
            await dishManagementService.DeleteDishAsync(id);
            return RedirectToAction("DishesManagement", new { ownerId });
        }
    }
}
