using DeliveryService.Domain.Common.Exceptions;
using DeliveryService.Domain.Common.ValueObjects;
using DeliveryService.Domain.Entities.Shipments;
using DeliveryService.Domain.Entities.Shipments.Enums;
using DeliveryService.Domain.Entities.Shipments.Events;
using DeliveryService.Domain.Entities.Shipments.Exceptions;
using FluentAssertions;
using Xunit;

namespace DeliveryService.Domain.UnitTests.Entities;

public class ShipmentTests
{
    private readonly Guid _validOrderId = Guid.NewGuid();
    private readonly string _validRecipientName = "Mohammad Arafat";
    private readonly string _validRecipientPhone = "+962790000000";
    private readonly decimal _validTotalAmount = 49.99m;

    private static AddressSnapshot CreateValidAddress() =>
        AddressSnapshot.Create(
            "Zahran Street",
            "Building 12, Apt 4",
            "Amman",
            "Amman Governorate",
            "Jordan"
        );

    private Shipment CreateValidShipment(decimal totalAmount = 49.99m) =>
        Shipment.Create(
            _validOrderId,
            _validRecipientName,
            _validRecipientPhone,
            CreateValidAddress(),
            totalAmount
        );

    [Fact]
    public void Create_WithValidParameters_ShouldCreateShipmentWithPendingStatusAndTrackingNumber()
    {
        var shipment = CreateValidShipment();

        shipment.Should().NotBeNull();
        shipment.Id.Should().NotBeEmpty();
        shipment.OrderId.Should().Be(_validOrderId);
        shipment.RecipientName.Should().Be(_validRecipientName);
        shipment.RecipientPhone.Should().Be(_validRecipientPhone);
        shipment.DeliveryAddress.Should().BeEquivalentTo(CreateValidAddress());
        shipment.TotalAmount.Should().Be(_validTotalAmount);
        shipment.Status.Should().Be(ShipmentStatus.Pending);
        shipment.TrackingNumber.Should().NotBeNull();
        shipment.TrackingNumber.Value.Should().StartWith("VEL-");
        shipment.DriverId.Should().BeNull();
        shipment.PickedUpAt.Should().BeNull();
        shipment.DeliveredAt.Should().BeNull();
        shipment.FailureReason.Should().BeNull();
        shipment.DeliveryAttempts.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithEmptyOrderId_ShouldThrowRequiredFieldException()
    {
        var act = () =>
            Shipment.Create(
                Guid.Empty,
                _validRecipientName,
                _validRecipientPhone,
                CreateValidAddress(),
                _validTotalAmount
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*orderId*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceRecipientName_ShouldThrowRequiredFieldException(
        string? invalidName
    )
    {
        var act = () =>
            Shipment.Create(
                _validOrderId,
                invalidName!,
                _validRecipientPhone,
                CreateValidAddress(),
                _validTotalAmount
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*recipientName*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceRecipientPhone_ShouldThrowRequiredFieldException(
        string? invalidPhone
    )
    {
        var act = () =>
            Shipment.Create(
                _validOrderId,
                _validRecipientName,
                invalidPhone!,
                CreateValidAddress(),
                _validTotalAmount
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*recipientPhone*");
    }

    [Fact]
    public void Create_WithNullDeliveryAddress_ShouldThrowRequiredFieldException()
    {
        var act = () =>
            Shipment.Create(
                _validOrderId,
                _validRecipientName,
                _validRecipientPhone,
                null!,
                _validTotalAmount
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*deliveryAddress*");
    }

    [Fact]
    public void Create_WithNegativeTotalAmount_ShouldThrowInvalidValueException()
    {
        var act = () => CreateValidShipment(totalAmount: -1m);

        act.Should().Throw<InvalidValueException>().WithMessage("Total amount cannot be negative.");
    }

    [Fact]
    public void Create_ShouldTrimRecipientNameAndPhone()
    {
        var shipment = Shipment.Create(
            _validOrderId,
            "  Mohammad Arafat  ",
            "  +962790000000  ",
            CreateValidAddress(),
            _validTotalAmount
        );

        shipment.RecipientName.Should().Be("Mohammad Arafat");
        shipment.RecipientPhone.Should().Be("+962790000000");
    }

    [Fact]
    public void AssignDriver_WhenStatusIsPendingAndDriverIdIsValid_ShouldAssignDriverAndSetStatusToAssigned()
    {
        var shipment = CreateValidShipment();
        var driverId = Guid.NewGuid();

        shipment.AssignDriver(driverId);

        shipment.DriverId.Should().Be(driverId);
        shipment.Status.Should().Be(ShipmentStatus.Assigned);
    }

    [Fact]
    public void AssignDriver_WhenDriverIdIsEmpty_ShouldThrowRequiredFieldException()
    {
        var shipment = CreateValidShipment();

        var act = () => shipment.AssignDriver(Guid.Empty);

        act.Should().Throw<RequiredFieldException>().WithMessage("*driverId*");
    }

    [Theory]
    [InlineData(ShipmentStatus.Assigned)]
    [InlineData(ShipmentStatus.PickedUp)]
    [InlineData(ShipmentStatus.InTransit)]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Failed)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void AssignDriver_WhenStatusIsNotPending_ShouldThrowInvalidStatusException(
        ShipmentStatus initialStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, initialStatus);

        var act = () => shipment.AssignDriver(Guid.NewGuid());

        act.Should().Throw<InvalidStatusException>();
    }

    [Fact]
    public void PickUp_WhenStatusIsAssigned_ShouldSetPickedUpAtAndSetStatusToPickedUp()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        var beforeTime = DateTime.UtcNow;

        shipment.PickUp();

        shipment.Status.Should().Be(ShipmentStatus.PickedUp);
        shipment.PickedUpAt.Should().NotBeNull();
        shipment.PickedUpAt.Value.Should().BeOnOrAfter(beforeTime);
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.PickedUp)]
    [InlineData(ShipmentStatus.InTransit)]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Failed)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void PickUp_WhenStatusIsNotAssigned_ShouldThrowInvalidStatusException(
        ShipmentStatus initialStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, initialStatus);

        var act = () => shipment.PickUp();

        act.Should().Throw<InvalidStatusException>();
    }

    [Fact]
    public void StartTransit_WhenStatusIsPickedUp_ShouldSetStatusToInTransit()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        shipment.PickUp();

        shipment.StartTransit();

        shipment.Status.Should().Be(ShipmentStatus.InTransit);
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.Assigned)]
    [InlineData(ShipmentStatus.InTransit)]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Failed)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void StartTransit_WhenStatusIsNotPickedUp_ShouldThrowInvalidStatusException(
        ShipmentStatus initialStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, initialStatus);

        var act = () => shipment.StartTransit();

        act.Should().Throw<InvalidStatusException>();
    }

    [Fact]
    public void MarkDelivered_WhenStatusIsInTransit_ShouldSetDeliveredAtDeliveredStatusAndRaiseDomainEvent()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        shipment.PickUp();
        shipment.StartTransit();
        var beforeTime = DateTime.UtcNow;

        shipment.MarkDelivered();

        shipment.Status.Should().Be(ShipmentStatus.Delivered);
        shipment.DeliveredAt.Should().NotBeNull();
        shipment.DeliveredAt.Value.Should().BeOnOrAfter(beforeTime);

        shipment.DomainEvents.Should().ContainSingle(e => e is ShipmentDeliveredEvent);
        var domainEvent = shipment.DomainEvents.OfType<ShipmentDeliveredEvent>().Single();
        domainEvent.ShipmentId.Should().Be(shipment.Id);
        domainEvent.OrderId.Should().Be(shipment.OrderId);
        domainEvent.DeliveredAt.Should().Be(shipment.DeliveredAt.Value);
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.Assigned)]
    [InlineData(ShipmentStatus.PickedUp)]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Failed)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void MarkDelivered_WhenStatusIsNotInTransit_ShouldThrowInvalidStatusException(
        ShipmentStatus initialStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, initialStatus);

        var act = () => shipment.MarkDelivered();

        act.Should().Throw<InvalidStatusException>();
    }

    [Theory]
    [InlineData(ShipmentStatus.Assigned)]
    [InlineData(ShipmentStatus.PickedUp)]
    [InlineData(ShipmentStatus.InTransit)]
    public void MarkFailed_WhenStatusIsActive_ShouldSetFailedStatusFailureReasonAndAddDeliveryAttempt(
        ShipmentStatus activeStatus
    )
    {
        var shipment = CreateValidShipment();
        var driverId = Guid.NewGuid();
        shipment.AssignDriver(driverId);

        if (activeStatus is ShipmentStatus.PickedUp or ShipmentStatus.InTransit)
            shipment.PickUp();
        if (activeStatus is ShipmentStatus.InTransit)
            shipment.StartTransit();

        const string failureReason = "Customer was not available";

        shipment.MarkFailed(failureReason);

        shipment.Status.Should().Be(ShipmentStatus.Failed);
        shipment.FailureReason.Should().Be(failureReason);
        shipment.DeliveryAttempts.Should().HaveCount(1);

        var attempt = shipment.DeliveryAttempts.Single();
        attempt.ShipmentId.Should().Be(shipment.Id);
        attempt.DriverId.Should().Be(driverId);
        attempt.FailureReason.Should().Be(failureReason);
        attempt.AttemptedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void MarkFailed_WhenStatusIsNotActive_ShouldThrowShipmentNotReadyException(
        ShipmentStatus inactiveStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, inactiveStatus);

        var act = () => shipment.MarkFailed("Some reason");

        act.Should()
            .Throw<ShipmentNotReadyException>()
            .WithMessage("Only an active shipment can be marked as failed.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MarkFailed_WithNullOrWhitespaceReason_ShouldThrowRequiredFieldException(
        string? invalidReason
    )
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());

        var act = () => shipment.MarkFailed(invalidReason!);

        act.Should().Throw<RequiredFieldException>().WithMessage("*reason*");
    }

    [Fact]
    public void Retry_WhenStatusIsNotFailed_ShouldThrowInvalidStatusException()
    {
        var shipment = CreateValidShipment();

        var act = () => shipment.Retry();

        act.Should().Throw<InvalidStatusException>();
    }

    [Fact]
    public void Retry_WhenFailedWithLessThanThreeDriverAttempts_ShouldKeepDriverAndSetStatusToAssigned()
    {
        var shipment = CreateValidShipment();
        var driverId = Guid.NewGuid();
        shipment.AssignDriver(driverId);
        shipment.MarkFailed("First attempt failed");

        shipment.Retry();

        shipment.Status.Should().Be(ShipmentStatus.Assigned);
        shipment.DriverId.Should().Be(driverId);
        shipment.FailureReason.Should().BeNull();
    }

    [Fact]
    public void Retry_WhenFailedWithThreeOrMoreDriverAttempts_ShouldUnassignDriverAndSetStatusToPending()
    {
        var shipment = CreateValidShipment();
        var driverId = Guid.NewGuid();

        shipment.AssignDriver(driverId);
        shipment.MarkFailed("Attempt 1 failed");
        shipment.Retry();

        shipment.MarkFailed("Attempt 2 failed");
        shipment.Retry();

        shipment.MarkFailed("Attempt 3 failed");

        shipment.Retry();

        shipment.Status.Should().Be(ShipmentStatus.Pending);
        shipment.DriverId.Should().BeNull();
        shipment.FailureReason.Should().BeNull();
        shipment.DeliveryAttempts.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.Assigned)]
    [InlineData(ShipmentStatus.PickedUp)]
    [InlineData(ShipmentStatus.InTransit)]
    [InlineData(ShipmentStatus.Failed)]
    public void Cancel_WhenStatusCanBeCancelled_ShouldSetStatusToCancelled(
        ShipmentStatus initialStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, initialStatus);

        shipment.Cancel();

        shipment.Status.Should().Be(ShipmentStatus.Cancelled);
    }

    [Theory]
    [InlineData(ShipmentStatus.Delivered)]
    [InlineData(ShipmentStatus.Cancelled)]
    public void Cancel_WhenStatusIsDeliveredOrCancelled_ShouldThrowInvalidStatusException(
        ShipmentStatus invalidStatus
    )
    {
        var shipment = CreateValidShipment();
        TransitionShipmentTo(shipment, invalidStatus);

        var act = () => shipment.Cancel();

        act.Should().Throw<InvalidStatusException>();
    }

    [Fact]
    public void ChangeStatus_ToPickedUp_ShouldTransitionToPickedUp()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());

        shipment.ChangeStatus(ShipmentStatus.PickedUp);

        shipment.Status.Should().Be(ShipmentStatus.PickedUp);
        shipment.PickedUpAt.Should().NotBeNull();
    }

    [Fact]
    public void ChangeStatus_ToInTransit_ShouldTransitionToInTransit()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        shipment.PickUp();

        shipment.ChangeStatus(ShipmentStatus.InTransit);

        shipment.Status.Should().Be(ShipmentStatus.InTransit);
    }

    [Fact]
    public void ChangeStatus_ToDelivered_ShouldTransitionToDelivered()
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        shipment.PickUp();
        shipment.StartTransit();

        shipment.ChangeStatus(ShipmentStatus.Delivered);

        shipment.Status.Should().Be(ShipmentStatus.Delivered);
        shipment.DeliveredAt.Should().NotBeNull();
    }

    [Fact]
    public void ChangeStatus_ToFailed_ShouldTransitionToFailed()
    {
        var shipment = CreateValidShipment();
        var driverId = Guid.NewGuid();
        shipment.AssignDriver(driverId);

        shipment.ChangeStatus(ShipmentStatus.Failed, "Delivery address not found");

        shipment.Status.Should().Be(ShipmentStatus.Failed);
        shipment.FailureReason.Should().Be("Delivery address not found");
        shipment.DeliveryAttempts.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(ShipmentStatus.Pending)]
    [InlineData(ShipmentStatus.Assigned)]
    public void ChangeStatus_ToPendingOrAssignedWhenFailed_ShouldRetry(ShipmentStatus targetStatus)
    {
        var shipment = CreateValidShipment();
        shipment.AssignDriver(Guid.NewGuid());
        shipment.MarkFailed("Address closed");

        shipment.ChangeStatus(targetStatus);

        shipment.Status.Should().Be(ShipmentStatus.Assigned);
        shipment.FailureReason.Should().BeNull();
    }

    [Fact]
    public void ChangeStatus_ToCancelled_ShouldTransitionToCancelled()
    {
        var shipment = CreateValidShipment();

        shipment.ChangeStatus(ShipmentStatus.Cancelled);

        shipment.Status.Should().Be(ShipmentStatus.Cancelled);
    }

    [Fact]
    public void ChangeStatus_WithInvalidTransition_ShouldThrowInvalidStatusException()
    {
        var shipment = CreateValidShipment();

        var act = () => shipment.ChangeStatus(ShipmentStatus.Delivered);

        act.Should().Throw<InvalidStatusException>();
    }

    private static void TransitionShipmentTo(Shipment shipment, ShipmentStatus targetStatus)
    {
        switch (targetStatus)
        {
            case ShipmentStatus.Pending:
                break;
            case ShipmentStatus.Assigned:
                shipment.AssignDriver(Guid.NewGuid());
                break;
            case ShipmentStatus.PickedUp:
                shipment.AssignDriver(Guid.NewGuid());
                shipment.PickUp();
                break;
            case ShipmentStatus.InTransit:
                shipment.AssignDriver(Guid.NewGuid());
                shipment.PickUp();
                shipment.StartTransit();
                break;
            case ShipmentStatus.Delivered:
                shipment.AssignDriver(Guid.NewGuid());
                shipment.PickUp();
                shipment.StartTransit();
                shipment.MarkDelivered();
                break;
            case ShipmentStatus.Failed:
                shipment.AssignDriver(Guid.NewGuid());
                shipment.MarkFailed("Failed attempt");
                break;
            case ShipmentStatus.Cancelled:
                shipment.Cancel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(targetStatus), targetStatus, null);
        }
    }
}
