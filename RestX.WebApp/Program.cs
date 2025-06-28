using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Services;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRepository, EntityFrameworkRepository<RestXRestaurantManagementContext>>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();

builder.Services.AddAutoMapper(typeof(Program)); // or (MappingProfile)

// Configure the new Code First DbContext
builder.Services.AddDbContext<RestXRestaurantManagementContext>(options =>
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
// Build port 5000
//builder.WebHost.UseUrls("http://0.0.0.0:5000");
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
    app.UseHsts();
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseHttpsRedirection(); // Chỉ redirect khi dev
//}

app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{ownerId?}/{tableId?}");
app.Run();
