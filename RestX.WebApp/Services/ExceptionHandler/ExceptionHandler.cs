using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RestX.WebApp.Services.Interfaces
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger logger;
        private IHttpContextAccessor httpContextAccessor;
        public ExceptionHandler(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor = null)
        {
            this.logger = loggerFactory.CreateLogger<ExceptionHandler>();
            this.httpContextAccessor = httpContextAccessor;
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
            // Có thể thêm log lỗi tại đây nếu cần
        }
    }
}
