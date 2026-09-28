using BuildingBlocks.Behaviors;
using BuildingBlocks.Extensions.ServiceCollection;
using BuildingBlocks.Middlewares;
using BuildingBlocks.User;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(cfg => cfg.Connection
    (builder.Configuration.GetConnectionString("DefaultConnection")!))
    .UseLightweightSessions();

builder.Services.AddSwagger("Product");

builder.Services.AddCarter();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddAuthorization();
builder.Services.AddAuthenticationService(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddGrpc();

var app = builder.Build();

app.UseSwagger();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapCarter();
app.MapGrpcService<ProductService.gRPC.Services.ProductService>();
app.Run();
