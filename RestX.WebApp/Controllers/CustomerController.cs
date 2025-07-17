using Microsoft.AspNetCore.Mvc;
using RestX.WebApp.Helper;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using RestX.WebApp.Services.Interfaces;
using System.Diagnostics;

namespace RestX.WebApp.Controllers
{
    [Route("Customer")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService customerService;
        private readonly IAuthCustomerService authCustomerService;

        public CustomerController(
            ICustomerService customerService,
            IAuthCustomerService authCustomerService,
            IExceptionHandler exceptionHandler)
            : base(exceptionHandler)
        {
            this.customerService = customerService;
            this.authCustomerService = authCustomerService;
        }

        #region Customer Management (Existing functionality)
        [HttpGet("Index")]
        public async Task<IActionResult> CustomersManagement()
        {
            try
            {
                var customers = await customerService.GetCustomersByOwnerIdAsync();
                var model = new CustomersManagementViewModel { Customers = customers };
                return View(model);
            }
            catch (Exception ex)
            {
                this.exceptionHandler.RaiseException(ex, "An error occurred while loading customers.");
                return View("Error");
            }
        }

        [HttpGet("Detail/{id:guid}")]
        public async Task<IActionResult> CustomerDetail(Guid id)
        {
            var customer = await customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return Json(new { success = false, message = "Customer not found." });
            return Json(new { success = true, data = customer });
        }

        [HttpPost("Upsert")]
        public async Task<IActionResult> UpsertCustomer([FromForm] CustomerViewModel model)
        {
            try
            {
                var resultId = await customerService.UpsertCustomerAsync(model);
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

        [HttpDelete("Delete/{id:guid}")]
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
        #endregion

        #region Customer Authentication (Transferred from AuthCustomerController)
        [HttpGet]
        [Route("Login/{ownerId:guid}/{tableId:int}")]
        public IActionResult Login(Guid ownerId, int tableId, [FromQuery] string? returnUrl)
        {
            var viewModel = CreateLoginViewModel(ownerId, tableId, returnUrl);
            ViewBag.TableId = tableId;

            return View(viewModel);
        }

        [HttpPost]
        [Route("Login/{ownerId:guid}/{tableId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Guid ownerId, int tableId, LoginViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TableId = tableId;
                return View(model);
            }

            try
            {
                var authCustomer = await authCustomerService.LoginOrCreateAsync(model, cancellationToken);

                SetCustomerSession(authCustomer);

                return RedirectAfterLogin(model.ReturnUrl, model.OwnerId, tableId);
            }
            catch (ArgumentException argEx)
            {
                exceptionHandler.RaiseException(argEx, $"Invalid login data for OwnerId: {ownerId}");
                ModelState.AddModelError("", argEx.Message);
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, $"Login error for OwnerId: {ownerId}, TableId: {tableId}");
                ModelState.AddModelError("", "Đã có lỗi xảy ra trong quá trình đăng nhập. Vui lòng thử lại.");
            }

            ViewBag.TableId = tableId;
            return View(model);
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout([FromQuery] Guid? ownerId, [FromQuery] int? tableId)
        {
            try
            {
                ClearCustomerSession();
                return RedirectAfterLogout(ownerId, tableId);
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, "Logout error");
                return RedirectToAction("Index", "Home", new { ownerId = Guid.Empty, tableId = 1 });
            }
        }

        [HttpGet]
        [Route("CheckPhone/{ownerId:guid}")]
        public async Task<IActionResult> CheckPhone(Guid ownerId, [FromQuery] string phone, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                    return Json(new { exists = false, name = "" });

                var customer = await authCustomerService.FindCustomerByPhoneAsync(phone, ownerId, cancellationToken);

                return Json(new
                {
                    exists = customer != null,
                    name = customer?.Name ?? ""
                });
            }
            catch (Exception ex)
            {
                exceptionHandler.RaiseException(ex, $"Error checking phone for OwnerId: {ownerId}");
                return Json(new { exists = false, name = "" });
            }
        }

        private LoginViewModel CreateLoginViewModel(Guid ownerId, int tableId, string? returnUrl)
        {
            return new LoginViewModel
            {
                OwnerId = ownerId,
                ReturnUrl = returnUrl ?? Url.Action("Index", "Home", new { ownerId, tableId })
            };
        }

        private void SetCustomerSession(Customer customer)
        {
            HttpContext.Session.SetString("AuthCustomerId", customer.Id.ToString());
            HttpContext.Session.SetString("AuthCustomerName", customer.Name ?? string.Empty);
            HttpContext.Session.SetString("AuthCustomerPhone", customer.Phone ?? string.Empty);
        }

        private void ClearCustomerSession()
        {
            HttpContext.Session.Clear();
        }

        private IActionResult RedirectAfterLogin(string? returnUrl, Guid ownerId, int tableId)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home", new { ownerId, tableId });
        }

        private IActionResult RedirectAfterLogout(Guid? ownerId, int? tableId)
        {
            if (ownerId.HasValue && tableId.HasValue && ownerId != Guid.Empty && tableId > 0)
            {
                return RedirectToAction("Index", "Home", new { ownerId = ownerId.Value, tableId = tableId.Value });
            }

            return RedirectToAction("Index", "Home", new { ownerId = Guid.Empty, tableId = 1 });
        }
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}