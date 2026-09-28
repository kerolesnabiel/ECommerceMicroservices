namespace BuildingBlocks.Exceptions;

public class InsufficientStockException : Exception
{
    public InsufficientStockException()
        : base("Insufficient stock available for the requested item.")
    { }

    public InsufficientStockException(string message)
        : base(message)
    { }

    public InsufficientStockException(int requestedQuantity, int availableStock)
        : base($"Insufficient stock: You requested {requestedQuantity} items, but only {availableStock} are available.")
    { }

    public InsufficientStockException(int requestedQuantity, int availableStock, int existingQuantity)
        : base($"You currently have {existingQuantity} of this item in your cart. You can only add {availableStock - existingQuantity} more, but you're trying to add {requestedQuantity}. Available stock: {availableStock}.")
    { }
}

