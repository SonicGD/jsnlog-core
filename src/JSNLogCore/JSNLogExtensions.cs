using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JSNLogCore
{
    public static class JSNLogExtensions
    {
        public static void AddJSNLog(this IServiceCollection services, JSNLogOptions options)
        {
            services.AddSingleton(options);
        }

        public static void AddJSNLog(this IServiceCollection services)
        {
            services.AddSingleton<JSNLogOptions>();
        }

        public static void UseJSNLog(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<JSNLogMiddleware>();
        }
    }
}