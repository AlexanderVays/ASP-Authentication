using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(config => {
                config.UseInMemoryDatabase("Users");
            });

            // AddIdentity registers the services
            services.AddIdentity<IdentityUser, IdentityRole>(config => 
            {
                config.Password.RequiredLength = 6;
                config.Password.RequireUppercase = false; //removes restrictions on a password
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config => 
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

