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
    public static void AddServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddMarten(cfg => cfg.Connection
            (configuration.GetConnectionString("Database")!))
            .UseLightweightSessions();

        services.AddCarter();
        services.AddScoped<ErrorHandlingMiddleware>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.Decorate<ICartRepository, CachedCartRepository>();

        services.AddStackExchangeRedisCache(options =>
            options.Configuration = configuration.GetConnectionString("Redis"));

        MappingProfile.Configure();

        services.AddGrpcClient<ProductServiceProtoClient>(options => options.Address = new Uri(configuration["ProductServiceUrl"]!))
        .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

        services.AddGrpcClient<PaymentServiceProtoClient>(options => options.Address = new Uri(configuration["PaymentServiceUrl"]!))
        .ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

        services.AddMassTransitService(configuration);
    }
}
