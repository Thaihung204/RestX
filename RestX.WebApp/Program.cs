using Microsoft.EntityFrameworkCore;
using RestX.WebApp.Models;
using RestX.WebApp.Services;
using RestX.WebApp.Services.Interfaces;
using RestX.WebApp.Services.Services;
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


builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IAuthCustomerService, AuthCustomerService>();
builder.Services.AddScoped<IRepository, EntityFrameworkRepository<RestXDbContext>>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IExceptionHandler, ExceptionHandler>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAutoMapper(typeof(Program));


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
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

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

app.Run();