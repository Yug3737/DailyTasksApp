using DailyTasksApp;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// var connectionString = builder.Configuration.GetConnectionString("ProdConnection")
//                        ?? throw new InvalidOperationException("Connection string" +
//                                                               "'DefaultConnection' not found.");

var connectionString = builder.Environment.IsProduction()
    ? builder.Configuration.GetConnectionString("ProdConnection")
      ?? throw new InvalidOperationException("ProdConnection not found in configuration.")
    : builder.Configuration.GetConnectionString("DefaultConnection")
      ?? throw new InvalidOperationException("DefaultConnection not found in configuration.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer((connectionString)));
var app = builder.Build();

// try
// {
//     using var scope = app.Services.CreateScope();
//     var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     db.Database.CanConnect();
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"DB ERROR: {ex.Message}");
//     throw;
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts(); // UseHsts likely causes problem on free tier of azurewebsites.net, hence have removed it.
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