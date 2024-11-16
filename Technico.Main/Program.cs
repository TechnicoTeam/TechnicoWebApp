using Technico.Main.Data;

using Microsoft.EntityFrameworkCore;
using Technico.Main.Repositories;
using Technico.Main.Repositories.Implementations;
using Technico.Main.Services;

using Technico.Main.Services.Implementations;
using Technico.Main.Validators.Implementations;
using Technico.Main.Validators;
using Technico.Main.Middleware;


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
builder.Services.AddScoped<IPropertyValidator, PropertyValidator>();

builder.Services.AddScoped<IRepairRepository, RepairRepository>();
builder.Services.AddScoped<IRepairService, RepairService>();

builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IOwnerService, OwnerService>();


builder.Services.AddTransient<GlobalMiddleware>();


//services end here.............
//build
var app = builder.Build();

app.UseMiddleware<GlobalMiddleware>();
app.UseMiddleware<NotFoundMiddleware>();

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
