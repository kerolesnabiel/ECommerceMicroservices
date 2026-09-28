namespace BuildingBlocks.Events;

public class NotificationEvent
{
    public Guid UserId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? TargetUrl { get; set; }
}
