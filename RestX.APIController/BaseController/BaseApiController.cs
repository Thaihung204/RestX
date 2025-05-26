// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="Tprofile Ltd">
//   Tprofile Ltd
// </copyright>
// <summary>
//   The base API controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.ApplicationInsights.DataContracts;

namespace Tprofile.App.Controllers.BaseControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Tprofile.BLL.Helpers;
    using Tprofile.BLL.Interfaces;
    using Tprofile.Models.Identity;
    using Tprofile.Models.Tenants;

    /// <summary>
    /// The base API controller.
    /// </summary>
    [ApiController]
    public class BaseApiController : Controller
    {
        /// <summary>
        /// The current tenant.
        /// </summary>
        public readonly ActiveTenant CurrentTenant;

        /// <summary>
        /// The exception handler.
        /// </summary>
        public readonly IExceptionHandler ExceptionHandler;

        /// <summary>
        /// The logger.
        /// </summary>
        public readonly ILogger Logger;

        /// <summary>
        /// The mapper.
        /// </summary>
        public readonly IMapper Mapper;

        /// <summary>
        /// The user manager.
        /// </summary>
        public readonly UserManager<ApplicationUser> UserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        /// <param name="exceptionHandler">
        /// The exception handler.
        /// </param>
        /// <param name="tenant">
        /// The tenant.
        /// </param>
        public BaseApiController(
            ILoggerFactory loggerFactory,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IExceptionHandler exceptionHandler,
            IEnumerable<ActiveTenant> tenant)
        {
            this.Logger = loggerFactory.CreateLogger<BaseApiController>();
            this.Mapper = mapper;
            this.UserManager = userManager;
            this.ExceptionHandler = exceptionHandler;
            this.CurrentTenant = tenant.FirstOrDefault();
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next) {
            try {
                if (this.CurrentTenant != null && HttpContext != null) {
                    var requestTelemetry = HttpContext?.Features?.Get<RequestTelemetry>();
                    if (requestTelemetry != null) {
                        requestTelemetry.Properties["TenantId"] = CurrentTenant.Id.ToString();
                        requestTelemetry.Properties["TenantName"] = CurrentTenant.Name;
                        requestTelemetry.Properties["BrandId"] = CurrentTenant.ActiveBrand.Id.ToString();
                        requestTelemetry.Properties["BrandName"] = CurrentTenant.ActiveBrand.Name;
                        var user = await GetCurrentUserAsync();
                        if (user != null) {
                            requestTelemetry.Properties["UserName"] = user.UserName;
                            requestTelemetry.Properties["UserId"] = user.Id;
                        }
                    }
                }
            } finally {
                await base.OnActionExecutionAsync(context, next);
            }
        }

        /// <summary>
        /// The get current user async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        internal Task<ApplicationUser> GetCurrentUserAsync() => this.UserManager.GetUserAsync(this.HttpContext.User);

        /// <summary>
        /// Checks whether the current user is an admin user, i.e has any of the following roles Agent, Super Admin, Admin
        /// </summary>
        /// <returns>True or False</returns>
        internal async Task<bool> IsCurrentUserAdminUser()
        {
            var rolesToCheck = UtilitiesHelper.AdminRoles.Split(",");
            var isAdmin = false;

            foreach (var role in rolesToCheck) {
                isAdmin = await UserManager.IsInRoleAsync((await this.GetCurrentUserAsync()), role);

                if (isAdmin) return true;
            }

            return isAdmin;
        }

        /// <summary>
        /// Checks whether the current user is an admin user, i.e has any of the following roles Agent, Super Admin, Admin
        /// </summary>
        /// <returns>True or False</returns>
        internal async Task<bool> IsCurrentUserTradeUser()
        {
            var rolesToCheck = UtilitiesHelper.TradeAdminRoles.Split(",");
            var isTrade = false;

            foreach (var role in rolesToCheck)
            {
                isTrade = await UserManager.IsInRoleAsync((await this.GetCurrentUserAsync()), role);

                if (isTrade) return true;
            }

            return isTrade;
        }
    }
}