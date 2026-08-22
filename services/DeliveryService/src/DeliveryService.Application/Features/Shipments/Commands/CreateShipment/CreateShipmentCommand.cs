using DeliveryService.Application.Features.Shipments.Dtos;
using DeliveryService.Domain.Common.ValueObjects;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Commands.CreateShipment;

public sealed record CreateShipmentCommand(
    Guid OrderId,
    string CustomerName,
    string CustomerPhone,
    AddressSnapshot ShippingAddress,
    decimal TotalAmount
) : IRequest<CreateShipmentDto>;
