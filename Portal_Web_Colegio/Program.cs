using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var useInMemory = builder.Configuration.GetValue("Database:UseInMemory", false);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (useInMemory)
    {
        options.UseInMemoryDatabase("PortalColegioLocal");
    }
    else if (!string.IsNullOrWhiteSpace(connectionString))
    {
        options.UseMySQL(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("PortalColegioLocal");
    }
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var providerName = db.Database.ProviderName ?? "";
    if (providerName.Contains("InMemory", StringComparison.OrdinalIgnoreCase))
    {
        db.Database.EnsureCreated();
    }
    else if (db.Database.IsRelational())
    {
        try
        {
            db.Database.Migrate();
        }
        catch (MySqlException ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
