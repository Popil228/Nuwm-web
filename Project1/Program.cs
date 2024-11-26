using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Íàëàøòóâàííÿ àâòåíòèô³êàö³¿ ÷åðåç Google
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
    options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
    options.CallbackPath = "/login/signin-google";
    options.SaveTokens = true; // Зберігати токени для відлагодження
    // Додаткова логіка після успішного входу
    options.Events.OnTicketReceived = context =>
    {
        var userPrincipal = context.Principal;
        context.HttpContext.Response.Headers.Add("Cache-Control", "no-store");

        // Зберігаємо в кеш, наприклад, на 30 хвилин
        // Можна використовувати MemoryCache або інший механізм кешування
        context.HttpContext.RequestServices.GetService<IMemoryCache>().Set("UserPrincipal", userPrincipal, TimeSpan.FromMinutes(30));
        return Task.CompletedTask;
    };    
});

builder.Services.AddAuthorization();

// Íàëàøòóâàííÿ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyOrigin()  // Дозволити доступ з будь-якого джерела
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
});

// Äîäàâàííÿ êîíòåêñòó áàçè äàíèõ òà àâòåíòèô³êàö³¿
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Äîäàâàííÿ Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// Íàëàøòóâàííÿ HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // Çà çàìîâ÷óâàííÿì HSTS äëÿ ïðîäóêö³éíîãî ñåðåäîâèùà
    app.UseHsts();
}

// Âèêîðèñòàííÿ CORS
app.UseCors("ReactPolicy");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Ïîðÿäîê âèêîðèñòàííÿ Middleware
app.UseAuthentication();  // Âèêîðèñòîâóºìî àâòåíòèô³êàö³þ
app.UseIdentityServer();   // Âèêîðèñòîâóºìî IdentityServer
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

// Âèçíà÷àºìî fallback äëÿ SPA
app.MapFallbackToFile("index.html");

app.MapControllers();

app.Run();
