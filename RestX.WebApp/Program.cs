using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Services;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
using RestX.WebApp.Filters;

namespace RestX.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews(options =>
            {
                // Đăng ký filter của chúng ta vào bộ sưu tập filter toàn cục
                // ASP.NET Core sẽ tự động tìm và khởi tạo nó cho mỗi request
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
