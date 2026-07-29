namespace Velora.Api.Contracts;

public sealed record AddCartItemRequest(Guid ProductId, int Quantity);

public sealed record RemoveCartItemRequest(Guid ProductId);
