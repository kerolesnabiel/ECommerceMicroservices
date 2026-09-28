using Carter;
using MediatR;

namespace NotificationService.Features.Notification.ReadNotifications;

public class ReadNotificationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/notifications", async (ISender sender) =>
        {
            await sender.Send(new ReadNotificationsCommand());
            return Results.NoContent();
        })
        .WithName("ReadNotifications")
        .Produces(StatusCodes.Status204NoContent)
        .RequireAuthorization();
    }
}
