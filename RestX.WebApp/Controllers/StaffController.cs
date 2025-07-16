using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
using System.Diagnostics;
using System.Linq;

namespace RestX.WebApp.Controllers
{
    [Authorize(Roles = "Staff")]

    public class StaffController : BaseController
    {
        public IMenuService menuService { get; }
        private readonly IStaffService staffService;
        private readonly ITableService tableService;
        private readonly IDishService dishService;

        public StaffController(IMenuService menuService, IStaffService staffService, ITableService tableService, IDishService dishService, IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.menuService = menuService;
            this.staffService = staffService;
            this.tableService = tableService;
            this.dishService = dishService;
        }

        [HttpGet]
        [Route("Staff/CustomerRequest")]
        public async Task<IActionResult> CustomerRequest(CancellationToken cancellationToken)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing Index for OwnerId: {OwnerId}");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        [Route("Staff/Menu")]
        public async Task<IActionResult> Menu(CancellationToken cancellationToken)
        {
            try
            {
                staffService.GetCurrentStaff(cancellationToken).Wait();
                var model = await menuService.GetMenuViewModelAsync(cancellationToken);
                return View(model);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing Index for OwnerId: {OwnerId}");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        [Route("Staff/StatusTable")]
        public async Task<IActionResult> StatusTable(CancellationToken cancellationToken)
        {
            try
            {
                var staff = await staffService.GetCurrentStaff(cancellationToken);
                var model = await tableService.GetAllTablesByOwnerIdAsync(staff.OwnerId, cancellationToken);
                return View(model);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing Index for OwnerId: {OwnerId}");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        [Route("Staff/Profile")]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            try
            {
                var staff = await staffService.GetStaffProfileAsync(cancellationToken);

                if (staff == null)
                {
                    return NotFound("Staff not found.");
                }

                return View(staff);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing Profile for Staff.");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost]
        [Route("Staff/UpdateDishAvailability")]
        public async Task<IActionResult> UpdateDishAvailability([FromBody] UpdateDishAvailabilityRequest request)
        {
            try
            {
                Console.WriteLine($"Received request: DishId={request.DishId}, IsActive={request.IsActive}");
                
                var success = await dishService.UpdateDishAvailabilityAsync(request.DishId, request.IsActive);
                
                Console.WriteLine($"Update result: {success}");
                
                if (success)
                {
                    return Json(new { success = true, message = "Dish availability updated successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to update dish availability. Dish not found or access denied." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateDishAvailability: {ex.Message}");
                this.exceptionHandler.RaiseException(ex, $"An error occurred while updating dish availability for DishId: {request.DishId}");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class UpdateDishAvailabilityRequest
    {
        public int DishId { get; set; }
        public bool IsActive { get; set; }
    }
}
