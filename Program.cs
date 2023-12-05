using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSupportTicketSystem.Web.Areas.Identity.Data;
using TheSupportTicketSystem.Web.Data;




var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});




builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


CreateRoles(app.Services).Wait();


app.Run();



async Task CreateRoles(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Client User", "Support Team User" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);

        if (!roleExist)
        {
          
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
