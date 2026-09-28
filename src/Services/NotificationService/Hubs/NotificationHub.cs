using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

namespace NotificationService.Hubs;

[Authorize]
public class NotificationHub(IDistributedCache cache) : Hub
{
    public async override Task OnConnectedAsync()
    {
        bool valid = Guid.TryParse(Context.UserIdentifier, out Guid userId);
        if (valid)
            await cache.SetStringAsync($"conn-{userId}", Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {

        bool valid = Guid.TryParse(Context.UserIdentifier, out Guid userId);
        if (valid)
            await cache.RemoveAsync($"conn-{userId}");

        await base.OnDisconnectedAsync(exception);
    }
}
