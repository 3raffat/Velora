using DeliveryService.Clients;
using DeliveryService.Contracts;
using DeliveryService.Data;
using DeliveryService.Entities.Shipments;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Services;

public interface IShipmentService
{
    Task<Shipment> CreateAsync(CreateShipmentRequest request, CancellationToken ct);
    Task DeliverAsync(Guid shipmentId, CancellationToken ct);
}

public sealed class ShipmentService(IDeliveryDbContext _context, VeloraClient _client)
    : IShipmentService
{
    public async Task<Shipment> CreateAsync(CreateShipmentRequest request, CancellationToken ct)
    {
        var shipment = Shipment.Create(
            request.OrderId,
            request.CustomerName,
            request.CustomerPhone,
            request.ShippingAddress,
            request.TotalAmount
        );

        await _context.Shipments.AddAsync(shipment);

        await _context.SaveChangesAsync(ct);

        // await _client.NotifyDeliveredAsync(
        //     shipment.OrderId,
        //     shipment.Id,
        //     shipment.TrackingNumber,
        //     ct
        // );
        return shipment;
    }

    public async Task DeliverAsync(Guid shipmentId, CancellationToken ct)
    {
        var shipment = await _context.Shipments.FirstOrDefaultAsync(x => x.Id == shipmentId, ct);

        if (shipment is null)
            throw new KeyNotFoundException($"Shipment with ID '{shipmentId}' was not found.");

        shipment.Deliver();

        await _context.SaveChangesAsync(ct);
    }
}
