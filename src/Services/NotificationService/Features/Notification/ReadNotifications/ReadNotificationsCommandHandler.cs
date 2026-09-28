using BuildingBlocks.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;

namespace NotificationService.Features.Notification.ReadNotifications;

public class ReadNotificationsCommandHandler(
    NotificationDbContext dbContext,
    IUserContext userContext) 
        : IRequestHandler<ReadNotificationsCommand>
{
    public async Task Handle(ReadNotificationsCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        var notifications = await dbContext.Notifications.Where(n => n.UserId == currentUser.Id && n.IsRead == false)
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
            notification.IsRead = true;

        dbContext.Notifications.UpdateRange(notifications);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
