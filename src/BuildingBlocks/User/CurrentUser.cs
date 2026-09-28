namespace BuildingBlocks.User;

public record CurrentUser(Guid Id, IReadOnlyCollection<string> Roles)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}