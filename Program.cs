using EquipmentManager.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"));
});
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Btl.Session";
    options.IdleTimeout = TimeSpan.FromHours(20);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "san-pham/{id?}",
    defaults: new { controller = "Product", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "dang-nhap/{id?}",
    defaults: new { controller = "Login", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "dang-ki/{id?}",
    defaults: new { controller = "Register", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "gio-hang/{id?}",
    defaults: new { controller = "Cart", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "thiet-bi-xu-ly-nuoc/{id?}",
    defaults: new { controller = "Home", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
