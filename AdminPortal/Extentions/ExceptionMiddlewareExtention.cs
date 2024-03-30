using Admin.Portal.API.Handlers;

namespace Admin.Portal.API.Extentions
{
    public static class ExceptionMiddlewareExtention
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
