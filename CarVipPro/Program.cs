using CarVipPro.BLL.Interfaces;
using CarVipPro.BLL.Services;
using CarVipPro.DAL;
using CarVipPro.DAL.Interfaces;
using CarVipPro.DAL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CarVipPro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add context
            builder.Services.AddDbContext<CarVipProContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //DI
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<ICarCompanyRepository, CarCompanyRepository>();
            builder.Services.AddScoped<ICarCompanyService, CarCompanyService>();

            builder.Services.AddScoped<IVehicleCategoryRepository, VehicleCategoryRepository>();
            builder.Services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();

            builder.Services.AddScoped<IElectricVehicleRepository, ElectricVehicleRepository>();
            builder.Services.AddScoped<IElectricVehicleService, ElectricVehicleService>();

            builder.Services.AddRazorPages()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
    });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromDays(7);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
                ctx.Response.Headers["Pragma"] = "no-cache";
                ctx.Response.Headers["Expires"] = "0";
                await next();
            });

            app.MapRazorPages();

            app.Run();
        }
    }
}
