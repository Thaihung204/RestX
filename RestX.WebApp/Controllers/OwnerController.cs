using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using System.Diagnostics;
using RestX.WebApp.Helper;

namespace RestX.WebApp.Controllers
{
    public class OwnerController : BaseController
    {
        private readonly IDashboardService dashboardService;
        private readonly ICustomerService customerService;

        public OwnerController(
            IDashboardService dashboardService,

            IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.dashboardService = dashboardService;
            this.customerService = customerService;
        }
        
        public async Task<IActionResult> DashBoard(CancellationToken cancellationToken)
        {
            try
            { 
                var model = await dashboardService.GetDashboardViewModelAsync(cancellationToken);
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

        [HttpGet("Customers")]
        public async Task<IActionResult> CustomersManagement()
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();
                var customers = await customerService.GetCustomersByOwnerIdAsync(ownerId);
                var model = new CustomersManagementViewModel { Customers = customers };
                return View(model);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading customers.");
                return View("Error");
            }
        }

        [HttpGet("Customers/Detail/{id:guid}")]
        public async Task<IActionResult> CustomerDetail(Guid id)
        {
            var customer = await customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return Json(new { success = false, message = "Customer not found." });
            return Json(new { success = true, data = customer });
        }

        [HttpPost("Customers/Upsert")]
        public async Task<IActionResult> UpsertCustomer([FromForm] CustomerViewModel model)
        {
            try
            {
                var ownerId = GetOwnerIdFromClaim();
                var resultId = await customerService.UpsertCustomerAsync(model, ownerId);
                if (resultId == null)
                    return Json(new { success = false, message = "Operation failed." });

                string operation = model.Id != Guid.Empty ? "updated" : "created";
                return Json(new { success = true, message = $"Customer {operation} successfully!", customerId = resultId });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while saving the customer.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("Customers/Delete/{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var result = await customerService.DeleteCustomerAsync(id);
                if (!result)
                    return Json(new { success = false, message = "Customer not found." });
                return Json(new { success = true, message = "Customer deleted successfully!" });
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while deleting the customer.");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}