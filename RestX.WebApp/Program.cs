using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Filters;
using RestX.WebApp.Models;
using RestX.WebApp.Services;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
using RestX.WebApp.Hubs;
using RestX.WebApp.Services.SignalRLab;
using RestX.WebApp.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; 
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<RestaurantContextFilterAttribute>();
});
builder.Services.AddSignalR();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IAuthCustomerService, AuthCustomerService>();
builder.Services.AddScoped<IRepository, EntityFrameworkRepository<RestXRestaurantManagementContext>>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
options.LoginPath = "/Login/Login";
options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

builder.Services.AddAutoMapper(typeof(Program));
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
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
                                                                
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        
        context.Response.Redirect("/Home/Index/550E8400-E29B-41D4-A716-446655440040/1");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "home_with_params",
    pattern: "Home/Index/{ownerId:guid}/{tableId:int}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "login_with_params",
    pattern: "AuthCustomer/Login/{ownerId:guid}/{tableId:int}",
    defaults: new { controller = "AuthCustomer", action = "Login" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{ownerId?}/{tableId?}");

app.MapHub<SignalrServer>("/signalrServer");
app.MapHub<TableStatusHub>("/tableStatusHub");

app.Run();