using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text.Json;
using BuildingBlocks.User;

namespace BuildingBlocks.Extensions.ServiceCollection;

record JwtSettings(string publicKey, string issuer, string audience);

public static class AuthenticationExtension
{
    public static void AddAuthenticationService(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(async options =>
            {
                var publicKeyUrl = config["PublicKeyUrl"]!;
                string? publicKeyResponse = await FetchPublicKeyAsync(publicKeyUrl);

                if (string.IsNullOrEmpty(publicKeyResponse))
                {
                    throw new InvalidOperationException("Failed to fetch public key.");
                }

                var jwtSettings = JsonSerializer.Deserialize<JwtSettings>(publicKeyResponse);

                if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.publicKey))
                {
                    throw new InvalidOperationException("Invalid JWT settings.");
                }

                var rsa = RSA.Create();
                rsa.ImportFromPem(jwtSettings.publicKey);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.issuer,
                    ValidAudience = jwtSettings.audience,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
    }

    private static async Task<string?> FetchPublicKeyAsync(string url)
    {
        using var httpClient = new HttpClient();
        string response = string.Empty;

        for (int attempt = 0; attempt < 5; attempt++)
        {
            try
            {
                response = await httpClient.GetStringAsync(url);
                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching public key: {ex.Message}. Retrying... Attempt {attempt + 1}");
                await Task.Delay(5000);
            }
        }

        return null;
    }
}
