using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace calc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //            loggerFactory.AddConsole();

            //app.UseDeveloperExceptionPage();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Cowsay}/{action=What}/{id?}");
            });

            app.Run(async (context) =>
            {
                var host = context.Request.Host;
                var connection = context.Request.HttpContext.Connection;
                var message = $@"Request Host: {host}
Local IP: {connection.LocalIpAddress}
Local Port: {connection.LocalPort}
Remote IP: {connection.RemoteIpAddress}
Remote Port: {connection.RemotePort}
";
                await context.Response.WriteAsync(message);
            });
        }
    }
}
