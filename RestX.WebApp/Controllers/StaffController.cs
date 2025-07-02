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

        public StaffController(IMenuService menuService, IStaffService staffService, ITableService tableService, IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.menuService = menuService;
            this.staffService = staffService;
            this.tableService = tableService;
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
                var staffIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StaffId");
                if (staffIdClaim == null || !Guid.TryParse(staffIdClaim.Value, out Guid staffId))
                {
                    return Unauthorized("Staff ID not found in claims.");
                }

                var staff = await staffService.GetStaffByIdAsync(staffId, cancellationToken);
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
                var staffIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StaffId");
                if (staffIdClaim == null || !Guid.TryParse(staffIdClaim.Value, out Guid staffId))
                {
                    return Unauthorized("Staff ID not found in claims.");
                }
                var staff = await staffService.GetStaffByIdAsync(staffId, cancellationToken);

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
