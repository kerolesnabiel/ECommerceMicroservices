using BuildingBlocks.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;

namespace NotificationService.Features.Notification.GetNotifications;

public class GetNotificationsQueryHandler(NotificationDbContext dbContext, IUserContext userContext) 
        : IRequestHandler<GetNotificationsQuery, IEnumerable<Models.Notification>>
{
    public async Task<IEnumerable<Models.Notification>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        return await dbContext.Notifications.Where(n => n.UserId == currentUser.Id)
            .OrderByDescending(n => n.Timestamp)
            .ToListAsync(cancellationToken);
    }
}
