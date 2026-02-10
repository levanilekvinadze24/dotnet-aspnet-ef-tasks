using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.Repository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Add DbContext
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SportsStoreConnection")));

// Add repository
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped(SessionCart.GetCart);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
}

// Middleware
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Map routes
app.MapControllerRoute(
    name: "categoryPage",
    pattern: "{category}/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "category",
    pattern: "Products/{category}",
    defaults: new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute(
    name: "shoppingCart",
    pattern: "Cart/{action}/{id?}",
    defaults: new { controller = "Cart", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "checkout",
    pattern: "Checkout",
    defaults: new { controller = "Order", action = "Checkout" });

app.MapControllerRoute(
    name: "remove",
    pattern: "Cart/Remove",
    defaults: new { controller = "Cart", action = "Remove" });

app.MapControllerRoute(
    "error",
    "Error",
    new { Controller = "Home", action = "Error" });

SeedData.EnsurePopulated(app);
IdentitySeedData.EnsurePopulated(app);

app.Run();
