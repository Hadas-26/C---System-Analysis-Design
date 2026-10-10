using MedicaidEmploymentVerificationApplication.Models;
using MedicaidEmploymentVerificationApplication.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MevaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MevaContext")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<MevaContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddSession(option =>
{
    if(builder.Environment.IsDevelopment())
    {
        option.IdleTimeout = TimeSpan.FromMinutes(120); //longer timeout in development
    }
    else
    {
        option.IdleTimeout = TimeSpan.FromMinutes(30);
    }
     
    option.Cookie.HttpOnly = false;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    
    //Create Roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Applicant", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Could not create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }

    //Seed users
    if (app.Environment.IsDevelopment())
    {
        await DataSeeder.SeedIdentities(scope.ServiceProvider, app.Configuration); //Must seed after the roles are created
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
