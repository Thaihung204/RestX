using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Services.DataTransferObjects;
using RestX.WebApp.Services.Interfaces;
using System.Diagnostics;

namespace RestX.WebApp.Controllers
{
    public class StaffController : BaseController
    {
        public IMenuService menuService { get; }
        private readonly IStaffService staffService;
        private readonly ITableService tableService;
        private readonly IDishService dishService;
        private readonly IOrderService orderService;
        private readonly IOrderDetailService orderDetailService;

        public StaffController(IMenuService menuService, IStaffService staffService, ITableService tableService, IDishService dishService, IOrderService orderService, IOrderDetailService orderDetailService, IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.menuService = menuService;
            this.staffService = staffService;
            this.tableService = tableService;
            this.dishService = dishService;
            this.orderService = orderService;
            this.orderDetailService = orderDetailService;
        }

        [HttpGet]
        [Route("Staff/CustomerRequest")]
        public async Task<IActionResult> CustomerRequest(CancellationToken cancellationToken)
        {
            try
            {
                var model = await orderService.GetCustomerRequestsByStaffAsync(cancellationToken);
                return View(model);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while processing CustomerRequest for Staff");
                return this.BadRequest("An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        [Route("Staff/Menu")]
        public async Task<IActionResult> Menu(CancellationToken cancellationToken)
        {
            try
            {
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
                var model = await tableService.GetAllTablesByCurrentStaff(cancellationToken);
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
        public async Task<IActionResult> UpdateDishAvailability([FromBody] UpdateDishAvailability request)
        {
            try
            {                
                var success = await dishService.UpdateDishAvailabilityAsync(request.DishId, request.IsActive);        
                return Json(new { success = true, message = "Dish availability updated successfully." });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, $"An error occurred while updating dish availability for DishId: {request.DishId}");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost]
        [Route("Staff/UpdateOrderDetailStatus")]
        public async Task<IActionResult> UpdateOrderDetailStatus([FromBody] UpdateOrderDetailStatus request)
        {
            try
            {
                var success = await orderDetailService.UpdateOrderDetailStatusAsync(request.OrderDetailId, request.IsActive);
                return Json(new { success = success, message = success ? "Order detail status updated successfully." : "Failed to update order detail status." });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, $"An error occurred while updating order detail status for OrderDetailId: {request.OrderDetailId}");
                return Json(new { success = false, message = "An unexpected error occurred. Please try again later." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
