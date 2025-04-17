using IceSync.Common;
using IceSync.Data;
using IceSync.Interfaces;
using IceSync.Jobs;
using IceSync.Middlewares;
using IceSync.Repositories;
using IceSync.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
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

//Quartz
builder.Services.AddQuartz(q =>
{
    q.UsePersistentStore(options =>
    {
        options.UseSqlServer(sqlServerOptions =>
        {
            sqlServerOptions.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString")!;
            sqlServerOptions.TablePrefix = "QRTZ_";
        });
        options.UseNewtonsoftJsonSerializer();
        options.UseClustering();
    });

    var jobKey = new JobKey("WorkflowSyncJob");

    q.AddJob<WorkflowSyncJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("WorkflowSyncJob-trigger")
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromSeconds(30))
            .RepeatForever()));
});

// Add Quartz hosted service to manage jobs in the background
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<IUniversalLoaderService, UniversalLoaderService>();
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddDistributedMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workflows}/{action=Index}/{id?}");

app.Run();
