using DeliveryService.Application.Features.Shipments.Dtos;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.CreateShipment;

public sealed record CreateShipmentCommand(
    Guid OrderId,
    string RecipientName,
    string RecipientPhone,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string Country,
    decimal TotalAmount
) : IRequest<CreateShipmentDto>;
