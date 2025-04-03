using AutoMapper;
using Company.Kirollos.BLL;
using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Data.Contexts;
using Company.Kirollos.DAL.Models;
using Company.Kirollos.PL.Mapping;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Company.Kirollos.PL.Settings;
using Company.Kirollos.PL.Settings.Interface;

namespace Company.Kirollos.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow Dependency injection for department
            //builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();     // Allow Dependency injection for Employee

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            builder.Services.AddAutoMapper(typeof(DepartmentProfile));

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IMailService, MailService>(); 

            #region Life time 
            // 1. builder.Services.AddScoped();    : life time per Request - new object
            // 2. builder.Services.AddSingleton(); : life time per Operation
            // 3. builder.Services.AddTransient(); : life time per App 
            #endregion

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Auth/SignIn";
                config.AccessDeniedPath = "/Auth/AccessDenied";
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    googleOptions.CallbackPath = "/signin-google";
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
                    facebookOptions.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
                    facebookOptions.CallbackPath = "/signin-facebook";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
