using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using forex_app_service.Models;
using Microsoft.AspNetCore.Builder;

namespace forex_app_service.Middleware
{
    public static class RequestAllowUpdates
    {
        public static IApplicationBuilder UseAllowUpdates(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AllowUpdates>();
        }
    }
    public class AllowUpdates
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<Settings> _settings;


        public AllowUpdates(RequestDelegate next, IOptions<Settings> settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var verb = context.Request.Method;
            if(!_settings.Value.AllowUpdates && verb !="GET")
            {
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                return;
            }
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}