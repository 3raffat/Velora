namespace DeliveryService.Entities.Shipments;

public sealed record AddressSnapshot
{
    public string AddressLine1 { get; }
    public string? AddressLine2 { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }

    public AddressSnapshot(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string country
    )
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        Country = country;
    }
}
