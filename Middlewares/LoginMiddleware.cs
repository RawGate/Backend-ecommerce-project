using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.Middlewares
{
    public class LoginMiddleware
    {
        private readonly ILogger<LoginMiddleware> _logger;
        private RequestDelegate _next;

        public LoginMiddleware(ILogger<LoginMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"----Handling Request : {context.Request.Method} {context.Request.Path}----");
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation($"----Finished Handling Request : {context.Response.StatusCode}----");
            }
        }
    }
}