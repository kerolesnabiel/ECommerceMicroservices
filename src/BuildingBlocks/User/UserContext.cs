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

        var userIdClaim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        var roleClaim = user.FindFirst(c => c.Type == ClaimTypes.Role);
        var sellerIdClaim = user.FindFirst(c => c.Type == "SellerId");

        if (userIdClaim == null || roleClaim == null)
            throw new InvalidOperationException("Required claims are missing.");

        var userId = Guid.Parse(userIdClaim.Value);
        return new CurrentUser(userId, roleClaim.Value, sellerIdClaim?.Value);
    }
}
