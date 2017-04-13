using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JSNLog.Core.Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddJSNLog(new JSNLogOptions() {Route = "/logs"});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseJSNLog();

            app.Run(async context =>
            {
                await context.Response.WriteAsync(@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>JSNLog</title>
    <script type=""text/javascript"" src=""https://cdnjs.cloudflare.com/ajax/libs/jsnlog/2.22.1/jsnlog.min.js""></script>
    <script type=""text/javascript"">
        JL.setOptions({
            ""defaultAjaxUrl"": ""/logs""
        });
    </script>
</head>
<body>
<h1>Page with js error</h1>
<script>
    foo();
</script>
</body>
</html>
");
            });
        }
    }
}