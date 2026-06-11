using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie_API.Exe;
using MvcMovie_API.Middlewares;
using MvcMovie_API.Repositories;
using MvcMovie_API.Services;
using Scalar.AspNetCore;
//using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowUmbraco",
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:44380")
                            .AllowAnyHeader()
                            .AllowAnyMethod();                            
                      });
});

builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidation();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        In = ParameterLocation.Header,
        Description = "ApiKey must appear in header",
    });

    x.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("X-API-KEY", document)] = []
    });
});
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddOpenApi();


// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

//IBackgroundJobClient backgroundJob; 
//backgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
    app.MapScalarApiReference();
    //app.MapScalarApiReference(options =>
    //{
    //    options.WithTitle("HotWheels Collection API")
    //           .WithTheme(ScalarTheme.Moon)
    //           .ForceDarkMode()
    //           .HideClientButton()
    //           .AddPreferredSecuritySchemes("X-API-KEY")
    //           .AddHttpAuthentication("X-API-KEY", auth =>
    //           {
    //               auth.Token = "X-API-KEY";
    //               auth.Description = "Api key";
    //           });
    //});
}

app.UseCors("AllowUmbraco");

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseHangfireDashboard();

//for(int i = 0; i < 100; i++)
//{
//    BackgroundJob.Enqueue<Scheduler>(x => x.PostDataDefault());
//}

RecurringJob.AddOrUpdate<Scheduler>("Create Movie", x => x.PostDataDefault(), Cron.MinuteInterval(1));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();