namespace BuildingBlocks.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException()
        : base("You do not have permission to access this resource.")
    { }

    public ForbiddenException(string message) : base(message)
    { }

    public ForbiddenException(string resourceName, string action)
        : base($"Access to the resource '{resourceName}' is forbidden for the action '{action}'.")
    { }
}
