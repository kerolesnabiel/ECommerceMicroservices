namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base("Entity was not found")
    {}
    
    public NotFoundException(string message) : base(message)
    {}

    public NotFoundException(string name, string id)
        : base($"{name} with identifier: {id} was not found")
    {}
}
