using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApplication13.Configs;
using WebApplication13.Configs.Extentions;
using WebApplication13.Entities.Data;
using WebApplication13.Services.HighTempCityService;
using WebApplication13.Services.UserServices;

namespace WebApplication13
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;
        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TaskDbContext>(options => {
                options.UseSqlServer(_Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfire(x => x.UseSqlServerStorage(_Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            var appSettingsSection = _Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddOurAuthentication(appSettings);
            services.AddOurSwagger();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IHighTempCityService, HighTempCityService>();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseRouting();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}");

                routes.MapRoute(
                    name: "single",
                    template: "{controller=User}/{id}");
            });


        }
    }
}
