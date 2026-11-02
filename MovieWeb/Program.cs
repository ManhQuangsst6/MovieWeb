using Microsoft.EntityFrameworkCore;
using MovieWeb.Models;
using MovieWeb.Repository;

var builder = WebApplication.CreateBuilder(args);
var ConnectString = builder.Configuration.GetConnectionString("MovieDbContext");
if (string.IsNullOrWhiteSpace(ConnectString))
{
    throw new InvalidOperationException(
        "Configure ConnectionStrings:MovieDbContext in appsettings or the ConnectionStrings__MovieDbContext environment variable.");
}
builder.Services.AddDbContext<MovieDbContext>(x => x.UseSqlServer(ConnectString));
builder.Services.AddScoped<IPhimRepository, PhimRepository>();
builder.Services.AddScoped<ITheLoaiRespository, TheLoaiRespository>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//D:\Huong\MovieWeb\MovieWeb\MovieWeb.csproj
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSession();

app.Run();
