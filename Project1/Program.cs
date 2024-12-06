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
using Project1.Models.Entitys;

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

builder.Services.AddIdentity<Person, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>() 
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<Person, ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddTransient<EmailService>();


var app = builder.Build();

//Ролі
using (var scope = app.Services.CreateAsyncScope())
{
    var provider = scope.ServiceProvider;

    // Отримання RoleManager для створення ролей
    var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Student", "Teacher" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Отримання UserManager для призначення ролей користувачам
    var userManager = provider.GetRequiredService<UserManager<Person>>();

    // Список користувачів для призначення ролей
    var userRoles = new Dictionary<string, string[]>
    {
        { "galenukm@gmail.com", new[] { "Admin" } },
        { "haleniuk_ak23@nuwm.edu.ua", new[] { "Student" } },
        { "bachmaniuk_ak23@nuwm.edu.ua", new[] { "Student" } },
        { "parypa_ak23@nuwm.edu.ua", new[] { "Student" } },
        { "m.v.boichura@nuwm.edu.ua", new[] { "Teacher" } },
        { "a.v.shatna@nuwm.edu.ua", new[] { "Teacher" } },
        { "a.i.sydor@nuwm.edu.ua", new[] { "Teacher" } },
        { "glovogalenukm@gmail.com", new[] { "Teacher" } },
    };

    foreach (var (email, assignedRoles) in userRoles)
    {
        // Знайти користувача за email
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            foreach (var role in assignedRoles)
            {
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    await userManager.UpdateAsync(user);
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
        else
        {
            Console.WriteLine($"Користувача з email {email} не знайдено.");
        }
    }
}

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
app.UseDefaultFiles();
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
