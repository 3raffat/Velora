namespace Velora.Api.Contracts;

public sealed record LoginRequest(string Email, string Password);

public sealed record RegisterRequest(string Username, string Email, string Password);
