using MediatR;

namespace NotificationService.Features.Notification.ReadNotificationById;

public class ReadNotificationByIdCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
