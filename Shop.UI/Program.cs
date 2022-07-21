using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure;
using Shop.Application.UsersAdmin;
using Shop.Database;
using Shop.UI.Infrastructure;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin");
    options.Conventions.AuthorizePage("/Admin/ConfigureUsers", "Admin");
});
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(
        builder.Configuration["DefaultConnection"]
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    //options.AddPolicy("Manager", policy => policy.RequireClaim("Role", "Manager"));
    options.AddPolicy("Manager", policy => policy
        .RequireAssertion(context => 
            context.User.HasClaim("Role", "Admin")
            || context.User.HasClaim("Role", "Manager")));
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Cart";
    options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
});

builder.Services.AddTransient<ISessionManager, SessionManager>();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var adminUser = new IdentityUser
            {
                UserName = "Admin",
            };

            var managerUser = new IdentityUser
            {
                UserName = "Manager"
            };

            userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
            userManager.CreateAsync(managerUser, "password").GetAwaiter().GetResult();
            
            var adminClaim = new Claim("Role", "Admin");
            var managerClaim = new Claim("Role", "Manager");
            
            userManager.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
            userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();
        }
    }
} catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Admin}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();