using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ProveedorEntity.Models;
using reCAPTCHA.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProveedorDatosContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddRecaptcha(options =>
{
    options.SiteKey = builder.Configuration["Recaptcha:SiteKey"];
    options.SecretKey = builder.Configuration["Recaptcha:SecretKey"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Proveedors}/{action=Index}/{id?}");

app.Run();
