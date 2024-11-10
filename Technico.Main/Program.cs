using Technico.Main.Data;

using Microsoft.EntityFrameworkCore;
using Technico.Main.Repositories;
using Technico.Main.Repositories.Implementations;
using Technico.Main.Services;
using Technico.Main.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TechnicoDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("TechnicoWeb")));


builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
//builder.Services.AddScoped<IPropertyService, PropertyService>();


//services end here.............
//build
var app = builder.Build();


// Here we will write our interfaces and services 
//builder.Services.AddScoped<Interface, Service>(); - when we call an interface , a service comes

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();