using BuildingBlocks.Events;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Data;
using NotificationService.Hubs;

namespace NotificationService.Features.Notification.CreateNotification;

public class CreateNotificationConsumer(
    NotificationDbContext dbContext,
    IHubContext<NotificationHub> hubContext,
    IDistributedCache cache) 
        : IConsumer<NotificationEvent>
{
    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        var notification = context.Message.Adapt<Models.Notification>();
        dbContext.Notifications.Add(notification);
        await dbContext.SaveChangesAsync();

        await SendNotificationToUser(notification);
    }

    private async Task SendNotificationToUser(Models.Notification notification)
    {
        var connectionId = await cache.GetStringAsync($"conn-{notification.UserId}");

        if (!string.IsNullOrEmpty(connectionId))
        {
            await hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification);
        }
    }
}
