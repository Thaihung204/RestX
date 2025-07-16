using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Models;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
using System.Diagnostics;

namespace RestX.WebApp.Controllers
{
    [Route("Table")]
    public class TableController : BaseController
    {
        private readonly ITableService tableService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public TableController(ITableService tableService,
                               IHttpContextAccessor httpContextAccessor,
                               IExceptionHandler exceptionHandler) : base(exceptionHandler)
        {
            this.tableService = tableService;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("createqr")]
        public async Task<IActionResult> TableQrCode(CancellationToken cancellationToken)
        {
            try
            {
                var user = Helper.UserHelper.HttpContextAccessor.HttpContext?.User;
                var ownerIdClaim = user?.FindFirst("OwnerId");
                var ownerIdString = ownerIdClaim.ToString();
                ownerIdString = ownerIdString.Substring(ownerIdString.IndexOf(": ") + 1);
                var ownerId = Guid.Parse(ownerIdString);

                
                var model = await tableService.GetTablesByOwnerIdAsync(ownerId);
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
