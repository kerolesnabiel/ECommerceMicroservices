using Carter;
using MediatR;

namespace NotificationService.Features.Notification.GetNotifications;

public class GetNotificationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/notifications", async (ISender sender) =>
        {
            var result = await sender.Send(new GetNotificationsQuery());
            return Results.Ok(result);
        })
        .WithName("GetNotifications")
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
