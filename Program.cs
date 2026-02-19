using AssetLocater.Domain.Persistence;
using AssetLocater.Domain.Repositories.Implementations;
using AssetLocater.Domain.Repositories.Interfaces;
using AssetLocater.Domain.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<FileDbContext>(options =>
{
    options.UseSqlite($"Data Source={DatabasePath.Get()}");
});
builder.Services.AddScoped<IFileRepository, SqliteFileRepository>();
builder.Services.AddScoped<FileService>();



builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 120 * 1024 * 1024;
});


builder.Services.AddAuthentication("AuthCookie")
    .AddCookie("AuthCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<AuthService>();
var app = builder.Build();


// 🔹 SINGLE database initialization point
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    DbInitializer.Initialize(db);
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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
