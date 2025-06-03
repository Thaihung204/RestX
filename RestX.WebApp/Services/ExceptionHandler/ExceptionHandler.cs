using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Tprofile.BLL.ExceptionHandling
{
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using Tprofile.BLL.Interfaces;
    

    public class ExceptionHandler:IExceptionHandler
    {
        private readonly ILogger logger;
        private IHttpContextAccessor httpContextAccessor;
        //private readonly ActiveTenant currentTenant = null;
        public ExceptionHandler(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor = null
            //, IEnumerable<ActiveTenant> tenant = null
            )
        {
            this.logger = loggerFactory.CreateLogger<ExceptionHandler>();
            this.httpContextAccessor = httpContextAccessor;
            //this.currentTenant = tenant?.FirstOrDefault();
        }

        public void RaiseException(Exception ex, string customMessage = "")
        {
            var url = string.Empty;
            if (httpContextAccessor?.HttpContext?.Request != null)
            {
                var request = httpContextAccessor.HttpContext.Request;

                var host = request.Host != null ? request.Host.Host : string.Empty;
                var path = request.Path != null ? request.Path.ToString() : string.Empty;
                var queryString = request.QueryString != null ? request.QueryString.ToString() : string.Empty;

                var uriBuilder = new UriBuilder
                {
                    Scheme = request.Scheme,
                    Host = host,
                    Path = path,
                    Query = queryString
                };
                url = uriBuilder.Uri.ToString();
            }
            this.logger.LogError(ex, customMessage, new { CurrentTenant = this.currentTenant, URL = url });
        }
    }
}
