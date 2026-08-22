using MediatR;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Features.Orders.Queries.GetShipmentByOrderId;

public sealed record GetShipmentByOrderIdQuery(Guid OrderId) : IRequest<ShipmentTrackingResponse>;
