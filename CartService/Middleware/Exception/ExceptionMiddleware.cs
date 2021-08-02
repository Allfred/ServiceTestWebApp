using System.Net;
using System.Threading.Tasks;
using CartService.Logging;
using CartService.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CartService.Middleware.Exception
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FileLogger> _fileLogger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<FileLogger> filelogger)
        {
            _fileLogger = filelogger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (System.Exception ex)
            {
                _fileLogger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}