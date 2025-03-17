using AutoMapper;
using Company.Kirollos.BLL.Interfaces;
using Company.Kirollos.BLL.Repositories;
using Company.Kirollos.DAL.Data.Contexts;
using Company.Kirollos.PL.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace Company.Kirollos.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow Dependency injection for department
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();     // Allow Dependency injection for Employee
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            builder.Services.AddAutoMapper(typeof(DepartmentProfile));
            // Life time 
            // 1. builder.Services.AddScoped();    : life time per Request - new object
            // 2. builder.Services.AddSingleton(); : life time per Operation
            // 3. builder.Services.AddTransient(); : life time per App

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
