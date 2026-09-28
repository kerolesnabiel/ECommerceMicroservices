using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Data.Seeders;
using NotificationService.Hubs;
using BuildingBlocks.Extensions.ServiceCollection;
using BuildingBlocks.Extensions;
using System.Reflection;
using Carter;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(config.GetConnectionString("Database")));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = config.GetConnectionString("Redis"));

builder.Services.AddSwagger("Notification");
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<Seeder>();
builder.Services.AddSignalR();
builder.Services.AddAuthenticationService(config);
builder.Services.AddMassTransitService(config, Assembly.GetExecutingAssembly());

builder.Services.AddCarter();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddCors(options =>
{
    var origins = config.GetSection("Origins").Get<string[]>();
    options.AddPolicy("AllowOrigins", policyBuilder =>
        policyBuilder.WithOrigins(origins!)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseCors("AllowOrigins");
app.MapCarter();
app.MapHub<NotificationHub>("/api/notifications/hub");
app.UseAuthentication();
app.UseAuthorization();
app.Run();
