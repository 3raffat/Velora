using DeliveryService.Domain.Common.Exceptions;
using DeliveryService.Domain.Entities.Shipments;
using FluentAssertions;
using Xunit;

namespace DeliveryService.Domain.UnitTests.Entities;

public class DeliveryAttemptTests
{
    private readonly Guid _validShipmentId = Guid.NewGuid();
    private readonly Guid _validDriverId = Guid.NewGuid();
    private readonly string _validFailureReason = "Customer was not available";

    [Fact]
    public void Create_WithValidParameters_ShouldCreateDeliveryAttempt()
    {
        var beforeTime = DateTime.UtcNow;

        var attempt = DeliveryAttempt.Create(_validShipmentId, _validDriverId, _validFailureReason);

        attempt.Should().NotBeNull();
        attempt.Id.Should().NotBeEmpty();
        attempt.ShipmentId.Should().Be(_validShipmentId);
        attempt.DriverId.Should().Be(_validDriverId);
        attempt.FailureReason.Should().Be(_validFailureReason);
        attempt.AttemptedAt.Should().BeOnOrAfter(beforeTime).And.BeOnOrBefore(DateTime.UtcNow);
        attempt.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyShipmentId_ShouldThrowRequiredFieldException()
    {
        var act = () => DeliveryAttempt.Create(Guid.Empty, _validDriverId, _validFailureReason);

        act.Should().Throw<RequiredFieldException>().WithMessage("*shipmentId*");
    }

    [Fact]
    public void Create_WithEmptyDriverId_ShouldThrowRequiredFieldException()
    {
        var act = () => DeliveryAttempt.Create(_validShipmentId, Guid.Empty, _validFailureReason);

        act.Should().Throw<RequiredFieldException>().WithMessage("*driverId*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceFailureReason_ShouldThrowRequiredFieldException(
        string? invalidReason
    )
    {
        var act = () => DeliveryAttempt.Create(_validShipmentId, _validDriverId, invalidReason!);

        act.Should().Throw<RequiredFieldException>().WithMessage("*failureReason*");
    }

    [Fact]
    public void Create_ShouldTrimFailureReason()
    {
        var attempt = DeliveryAttempt.Create(
            _validShipmentId,
            _validDriverId,
            "  Address not found  "
        );

        attempt.FailureReason.Should().Be("Address not found");
    }
}
