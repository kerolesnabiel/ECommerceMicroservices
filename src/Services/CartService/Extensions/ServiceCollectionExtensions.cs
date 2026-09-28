using BuildingBlocks.Middlewares;
using BuildingBlocks.Behaviors;
using CartService.Data;
using CartService.DTOs;
using static PaymentService.PaymentServiceProto;
using static ProductService.ProductServiceProto;
using BuildingBlocks.Extensions;

namespace CartService.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServiceExtensions(this WebApplicationBuilder builder)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(assembly);

        builder.Services.AddMarten(cfg => cfg.Connection
                (builder.Configuration.GetConnectionString("Database")!))
            .UseLightweightSessions();

        builder.Services.AddCarter();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();

        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.Decorate<ICartRepository, CachedCartRepository>();

        builder.Services.AddStackExchangeRedisCache(options =>
            options.Configuration = builder.Configuration.GetConnectionString("Redis"));

        MappingProfile.Configure();

        var handler = new HttpClientHandler();
        if (builder.Environment.IsDevelopment())
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

        builder.Services
            .AddGrpcClient<ProductServiceProtoClient>(options =>
                options.Address = new Uri(builder.Configuration["ProductServiceUrl"]!))
            .ConfigurePrimaryHttpMessageHandler(() => handler);

        builder.Services
            .AddGrpcClient<PaymentServiceProtoClient>(options =>
                options.Address = new Uri(builder.Configuration["PaymentServiceUrl"]!))
            .ConfigurePrimaryHttpMessageHandler(() => handler);

        builder.Services.AddMassTransitService(builder.Configuration);
    }
}
