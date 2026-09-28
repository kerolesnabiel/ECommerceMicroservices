using BuildingBlocks.Exceptions;
using BuildingBlocks.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;

namespace NotificationService.Features.Notification.ReadNotificationById;

public class ReadNotificationByIdCommandHandler(
    NotificationDbContext dbContext,
    IUserContext userContext) 
        : IRequestHandler<ReadNotificationByIdCommand>
{
    public async Task Handle(ReadNotificationByIdCommand request, CancellationToken cancellationToken)
    {
        var notification = await dbContext.Notifications.FirstOrDefaultAsync(n => n.Id ==  request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Models.Notification), request.Id.ToString());

        var currentUser = userContext.GetCurrentUser();
        if (notification.UserId != currentUser.Id)
            throw new ForbiddenException("You do not have permission to access this notification.");

        notification.IsRead = true;
        dbContext.Notifications.Update(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
