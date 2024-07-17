using System.Globalization;

namespace BlazorDemoApp.API.Helpers
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
           
            
            if (context.Request.Headers.ContainsKey("Accept-Language"))
            {
                var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
                var cultureName = acceptLanguage.Split(',')[0]; // Take the first language tag
                var culture = new CultureInfo(cultureName);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
           

            await _next(context);
        }
    }

    // Extension method to add the middleware to the pipeline
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
