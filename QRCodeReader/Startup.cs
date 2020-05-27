using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QRCodeReader.Models;
using QRCodeReader.Services;
using QRCodeReader.Common.Extensions;

namespace QRCodeReader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddTransient<IService<QRCodeRequest>, QRCodeService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();

            app.Run(async context =>
            {
                try
                {
                    var route = context.Request.Path.ToString().Split('/').FirstOrDefault(p => !string.IsNullOrEmpty(p));

                    switch (route)
                    {
                        case "qr":
                            {
                                await app.ApplicationServices.GetService<IService<QRCodeRequest>>().GetQRResult(context.Request.Query.FromQueryToObject<QRCodeRequest>());
                            }
                            break;
                        case "health":
                            context.Response.StatusCode = 200;
                            await context.Response.WriteAsync("healthy");
                            break;

                        default:
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsync("");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Routing exception :{ ex.Message}");
                    throw;
                }
            });
        }
    }
}
