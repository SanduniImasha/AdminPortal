using Admin.Portal.API.Core.Models.Base;
using Microsoft.Extensions.Options;
using System.Net;

namespace Admin.Portal.API.Handlers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Settings _config;
        public ExceptionMiddleware(RequestDelegate next, IOptions<Settings> config)
        {
            _next = next;
            _config = config.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    while (ex.InnerException != null) ex = ex.InnerException;

                    try
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(ex.Message);
                    }
                    catch(Exception exp)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await context.Response.WriteAsync(exp.Message);
                    }
                }
            }
        }
    }
}
