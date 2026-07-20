namespace Velora.Api.Contracts;

public sealed record CreateCategoryRequest(string Name, string Description);

public sealed record UpdateCategoryRequest(string Name, string Description);
