using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Features.Notification.ReadNotificationById;

public class ReadNotificationByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/notifications/{id}", 
        async ([FromRoute] Guid id, ISender sender) =>
        {
            await sender.Send(new ReadNotificationByIdCommand(id));
            return Results.NoContent();
        })
        .WithName("ReadNotification")
        .Produces(StatusCodes.Status200OK)
        .RequireAuthorization();
    }
}
