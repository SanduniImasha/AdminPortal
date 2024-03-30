using Admin.Portal.API.Handlers;

namespace Admin.Portal.API.Extentions
{
    public static class EnableRequestRewindMiddlewareExtention
    {
        public static void UseEnableRequestRewindMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<EnableRequestRewindMiddleware>();
        }
    }
}
