using EasyZap.Data;
using EasyZap.Models;
using EasyZap.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // builder web приложения

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EasyZapContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserService>();

var app = builder.Build(); // web приложение

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// middleware


app.UseStaticFiles(); // <- важно для CSS/JS
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseCookiePolicy(); // надо проверить

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // запуск приложения