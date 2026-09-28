using BuildingBlocks.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Text.Json;

namespace BuildingBlocks.Extensions.ServiceCollection;

public static class AuthenticationExtension
{
    public static void AddAuthenticationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = config["Keycloak:Authority"];
                options.Audience = config["Keycloak:Audience"];
                options.RequireHttpsMetadata = !config.GetValue<bool>("Keycloak:DevMode");
                options.MapInboundClaims = false;
            });

        services.AddSingleton<IClaimsTransformation, KeycloakRolesClaimsTransformation>();
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
    }
}

public class KeycloakRolesClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;

        if (identity.HasClaim(c => c.Type == ClaimTypes.Role))
            return Task.FromResult(principal);

        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (realmAccess != null)
        {
            using var doc = JsonDocument.Parse(realmAccess);
            if (doc.RootElement.TryGetProperty("roles", out var roles))
            {
                foreach (var role in roles.EnumerateArray())
                {
                    var roleName = role.GetString();
                    if (roleName != null)
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                }
            }
        }

        return Task.FromResult(principal);
    }
}