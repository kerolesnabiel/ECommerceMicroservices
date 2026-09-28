using MediatR;

namespace NotificationService.Features.Notification.GetNotifications;

public class GetNotificationsQuery : IRequest<IEnumerable<Models.Notification>>
{
}
