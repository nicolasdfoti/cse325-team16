using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Daily_Notes.Data;
using Daily_Notes.Models;

var builder = WebApplication.CreateBuilder(args);
// Registramos el "fake" ApplicationDbContext como Singleton
builder.Services.AddSingleton<ApplicationDbContext>();

// DB (DESACTIVADA)
// var conn = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(conn));

// Identity (DESACTIVADA)
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();
app.UseRouting();

// Autenticación eliminada
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
