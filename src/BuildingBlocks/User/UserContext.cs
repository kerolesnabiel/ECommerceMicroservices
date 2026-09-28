using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.User;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User
                   ?? throw new InvalidOperationException("User context is not present");

        if (user.Identity == null || !user.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException();

        var userIdClaim = user.FindFirst("sub");

        if (userIdClaim == null)
            throw new InvalidOperationException("Required claims are missing.");

        if (!Guid.TryParse(userIdClaim.Value, out var userId))
            throw new BadHttpRequestException("Invalid user id");

        var roles = user.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return new CurrentUser(userId, roles);
    }
}
