using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Technico.Main.Data;
using Technico.Main.Models;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Here is our database connection 
builder.Services.AddDbContext<TechnicoDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("TechnicoWeb")));




// Here we will write our interfaces and services 
//builder.Services.AddScoped<Interface, Service>(); - when we call an interface , a service comes

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

app.Run();