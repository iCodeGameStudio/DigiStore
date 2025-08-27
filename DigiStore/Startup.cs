using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DigiStore.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.Core.Classes;

namespace DigiStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
                options.LogoutPath = "/Accounts/LogOut";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });
            services.AddDbContext<DigiStore.DataAccessLayer.Context.DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<IUser, UserService>();
            services.AddTransient<IAccount, AccountService>();
            services.AddTransient<IAdmin, AdminService>();
            services.AddTransient<IStore, StoreService>();
            services.AddTransient<ITemp, TempService>();
            services.AddTransient<IViewRenderService,RenderToString>();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddScoped<PanelLayoutScope>();
            services.AddScoped<SiteLayoutScope>();
            services.AddScoped<MessageSender>();
        }

        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseRouting();
            app.UseStaticFiles();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World");
                });

            });
        } 
    }
}
