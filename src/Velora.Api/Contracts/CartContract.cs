namespace Velora.Api.Contracts;

public sealed record AddCartItemRequest(Guid CustomerId, Guid ProductId, int Quantity);

public sealed record RemoveCartItemRequest(Guid CustomerId, Guid CartId, Guid ProductId);
