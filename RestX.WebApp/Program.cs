using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Services;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;

namespace RestX.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IOwnerService, Services.Services.OwnerService>();
            builder.Services.AddScoped<ICustomerService, Services.Services.CustomerService>();
            builder.Services.AddScoped<IRepository, EntityFrameworkRepository<RestXDbContext>>();
            builder.Services.AddScoped<IDishService, Services.Services.DishService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

            builder.Services.AddAutoMapper(typeof(Program)); // or (MappingProfile)

            // Configure the new Code First DbContext
            builder.Services.AddDbContext<RestXDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestX"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                
                // Enable sensitive data logging in development
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });
            // Buld port 5000
            builder.WebHost.UseUrls("https://0.0.0.0:5000");
            // Keep the old DbContext for compatibility during migration
            builder.Services.AddDbContext<RestXRestaurantManagementContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestX"));
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{ownerId?}/{tableId?}");
            app.Run();
        }
    }
}
