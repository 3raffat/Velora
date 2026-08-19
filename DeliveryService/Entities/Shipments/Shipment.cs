using DeliveryService.Entities.Shipments.Enums;
using DeliveryService.Entities.Shipments.Exceptions;

namespace DeliveryService.Entities.Shipments;

public sealed class Shipment
{
    private Shipment() { }

    private Shipment(
        Guid orderId,
        string customerName,
        string customerPhone,
        AddressSnapshot shippingAddress,
        decimal totalAmount
    )
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        CustomerName = customerName;
        CustomerPhone = customerPhone;
        ShippingAddress = shippingAddress;
        TotalAmount = totalAmount;
        TrackingNumber = GenerateTrackingNumber();
        Status = ShipmentStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public string TrackingNumber { get; private set; } = null!;

    public string CustomerName { get; private set; } = null!;

    public string CustomerPhone { get; private set; } = null!;

    public AddressSnapshot ShippingAddress { get; private set; } = null!;

    public decimal TotalAmount { get; private set; }

    public ShipmentStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? DeliveredAt { get; private set; }

    public static Shipment Create(
        Guid orderId,
        string customerName,
        string customerPhone,
        AddressSnapshot shippingAddress,
        decimal totalAmount
    )
    {
        Validate(orderId, customerName, customerPhone, shippingAddress, totalAmount);
        return new Shipment(orderId, customerName, customerPhone, shippingAddress, totalAmount);
    }

    public void PickUp()
    {
        EnsureStatus(ShipmentStatus.Created);

        Status = ShipmentStatus.PickedUp;
    }

    public void MarkInTransit()
    {
        EnsureStatus(ShipmentStatus.PickedUp);

        Status = ShipmentStatus.InTransit;
    }

    public void MarkOutForDelivery()
    {
        EnsureStatus(ShipmentStatus.InTransit);

        Status = ShipmentStatus.OutForDelivery;
    }

    public void Deliver()
    {
        EnsureStatus(ShipmentStatus.OutForDelivery);

        Status = ShipmentStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
    }

    private static void Validate(
        Guid orderId,
        string customerName,
        string customerPhone,
        AddressSnapshot shippingAddress,
        decimal totalAmount
    )
    {
        if (orderId == Guid.Empty)
            throw new ShipmentException("Order ID cannot be empty.");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new ShipmentException("Customer name is required.");

        if (string.IsNullOrWhiteSpace(customerPhone))
            throw new ShipmentException("Customer phone is required.");

        if (shippingAddress is null)
            throw new ShipmentException("Shipping address is required.");

        if (totalAmount < 0)
            throw new ShipmentException("Total amount cannot be negative.");
    }

    private void EnsureStatus(ShipmentStatus expectedStatus)
    {
        if (Status != expectedStatus)
        {
            throw new ShipmentException($"Shipment must be in {expectedStatus} status.");
        }
    }

    private static string GenerateTrackingNumber()
    {
        return $"TRK-{Random.Shared.Next(100000, 999999)}";
    }
}
