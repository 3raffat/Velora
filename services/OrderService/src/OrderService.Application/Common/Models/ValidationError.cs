namespace OrderService.Application.Common.Models;

public sealed record ValidationError(string Message, string PropertyName);
