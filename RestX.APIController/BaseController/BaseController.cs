using AutoMapper;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tprofile.BLL.Interfaces;
using Tprofile.Models.Identity;
using Tprofile.Models.Tenants;

namespace Tprofile.App.Controllers.BaseControllers
{
    public class BaseController : Controller
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
        public BaseController(
            ILoggerFactory loggerFactory,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IExceptionHandler exceptionHandler,
            IEnumerable<ActiveTenant> tenant) {
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
                    if (requestTelemetry != null)
                    {
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

        protected string GetPath(string[] routeParamList)
        {
            var domain = this.HttpContext.Request.Host.Value;

            var path = this.HttpContext.Request.Path.Value;
            if (routeParamList.Length > 0 && CurrentTenant.ActiveBrand.Hostnames.Any(h => h.Hostname == $"{domain}/{routeParamList[0]}"))
            {
                // This is a tenant where the domain has a path, move along the routeParams
                path = "/" + string.Join("/", routeParamList.Skip(1).ToList());
            }

            return path;
        }

        /// <summary>
        /// The get current user async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        internal Task<ApplicationUser> GetCurrentUserAsync() => this.UserManager.GetUserAsync(this.HttpContext.User);
    }
}
