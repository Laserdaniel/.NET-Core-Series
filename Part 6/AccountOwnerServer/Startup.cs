using AccountOwnerServer.Extensions;
using AccountOwnerServer.Filters;
using AccountOwnerServer.Middleware;
using Entities.ExtendedModels;
using Entities.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using NLog;
using System;
using System.IO;
using System.Web.Http;

namespace AccountOwnerServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.ConfigureIISIntegration();

            services.ConfigureLoggerService();

            services.ConfigureSqlContext(Configuration);

            services.ConfigureRepositoryWrapper();

            services.AddMvc(options =>
                {
                    options.Filters.Add(typeof(UnhandledExceptionFilter));
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.ConfigureODataService();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseMiddleware<ResponseWrapper>();

            app.UseMvc(b =>
            {
                b.EnableDependencyInjection();
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                b.MapODataServiceRoute("api", "api", GetEdmModel());
            });
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Owner>("Owner");
            return builder.GetEdmModel();
        }


    }
}
