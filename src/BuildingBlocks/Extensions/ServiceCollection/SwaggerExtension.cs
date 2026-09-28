using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Reflection.Metadata;

namespace BuildingBlocks.Extensions.ServiceCollection;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services, string serviceName, int version = 1)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc($"v{version}", new OpenApiInfo
            {
                Title = serviceName + $" Service v{version}",
                Version = $"v{version}",
            });

            opts.AddServer(new OpenApiServer { Url = $"/{serviceName.ToLower()}-service" });

            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            opts.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
    }
}
