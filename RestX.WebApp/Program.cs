using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Filters;
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

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<RestaurantContextFilterAttribute>();
            });
            builder.Services.AddScoped<IOwnerService, OwnerService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IRepository, EntityFrameworkRepository<RestXDbContext>>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
            builder.Services.AddScoped<IMenuService, MenuService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            builder.Services.AddScoped<IIngredientImportService, IngredientImportService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IDishManagementService, DishManagementService>();
            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

            builder.Services.AddAutoMapper(typeof(Program));

            // File upload configuration
            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            // Configure the new Code First DbContext
            builder.Services.AddDbContext<RestXDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestX"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromDays(1),
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
            //builder.WebHost.UseUrls("https://0.0.0.0:5000");
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

            // Thêm middleware để chuyển hướng
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/Home/Index/550E8400-E29B-41D4-A716-446655440040/1");
                    return;
                }
                await next();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{ownerId?}/{tableId?}");
            app.Run();
            //this is comment
        }
    }
}
