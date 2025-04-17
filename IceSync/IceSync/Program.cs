using IceSync.Common;
using IceSync.Data;
using IceSync.Interfaces;
using IceSync.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<UniversalLoaderApiSettings>(builder.Configuration.GetSection("UniversalLoaderApi"));

//Refit
builder.Services.AddRefitClient<IUniversalLoaderExternalApi>()
    .ConfigureHttpClient((serviceProvider, client) =>
    {
        var settings = serviceProvider
            .GetRequiredService<IOptions<UniversalLoaderApiSettings>>().Value;

        client.BaseAddress = new Uri(settings.BaseUrl);
    });

builder.Services.AddScoped<IUniversalLoaderService, UniversalLoaderService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workflows}/{action=Index}/{id?}");

app.Run();
