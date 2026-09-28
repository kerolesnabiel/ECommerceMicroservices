using BuildingBlocks.Middlewares;
using BuildingBlocks.Extensions.ServiceCollection;

namespace UserService.Presentation.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddAuthenticationService(builder.Configuration);
    }
}
