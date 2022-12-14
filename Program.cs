using DMSystemMvc;
using DMSystemMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DeliveryManagementSystemContext>(options =>
              options.UseSqlServer("Server=YKG-PC;Database=DeliveryManagementSystem;Trusted_Connection=True;"));

builder.Services.AddSession();


builder.Services.AddControllersWithViews();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<EHandlingMiddleware>();

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
app.UseSession();

app.UseAuthorization();
app.UseMiddleware<EHandlingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Registration}/{action=Login}/{id?}");

app.Run();
