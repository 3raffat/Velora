using DeliveryService.Domain.Common.Exceptions;
using DeliveryService.Domain.Entities.Shipments.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryService.Domain.UnitTests.ValueObjects;

public class TrackingNumberTests
{
    [Fact]
    public void Generate_ShouldReturnValidTrackingNumberFormat()
    {
        var trackingNumber = TrackingNumber.Generate();

        trackingNumber.Should().NotBeNull();
        trackingNumber.Value.Should().NotBeNullOrWhiteSpace();
        trackingNumber.Value.Should().StartWith("VEL-");
        trackingNumber.Value.Length.Should().Be(22);
        trackingNumber.Value.Should().Be(trackingNumber.Value.ToUpperInvariant());
    }

    [Fact]
    public void Generate_CalledMultipleTimes_ShouldGenerateUniqueValues()
    {
        var trackingNumber1 = TrackingNumber.Generate();
        var trackingNumber2 = TrackingNumber.Generate();

        trackingNumber1.Value.Should().NotBe(trackingNumber2.Value);
    }

    [Fact]
    public void Create_WithValidValue_ShouldCreateTrackingNumberUpperInvariantAndTrimmed()
    {
        var trackingNumber = TrackingNumber.Create("  vel-20260830-abc12345  ");

        trackingNumber.Should().NotBeNull();
        trackingNumber.Value.Should().Be("VEL-20260830-ABC12345");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceValue_ShouldThrowRequiredFieldException(
        string? invalidValue
    )
    {
        var act = () => TrackingNumber.Create(invalidValue!);

        act.Should().Throw<RequiredFieldException>().WithMessage("*value*");
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        var trackingNumber = TrackingNumber.Create("VEL-20260830-12345678");

        trackingNumber.ToString().Should().Be("VEL-20260830-12345678");
    }

    [Fact]
    public void Equality_WithSameValue_ShouldBeEqual()
    {
        var trackingNumber1 = TrackingNumber.Create("VEL-20260830-12345678");
        var trackingNumber2 = TrackingNumber.Create("VEL-20260830-12345678");

        trackingNumber1.Should().Be(trackingNumber2);
        (trackingNumber1 == trackingNumber2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WithDifferentCasing_ShouldBeEqualDueToNormalization()
    {
        var trackingNumber1 = TrackingNumber.Create("vel-20260830-12345678");
        var trackingNumber2 = TrackingNumber.Create("VEL-20260830-12345678");

        trackingNumber1.Should().Be(trackingNumber2);
    }

    [Fact]
    public void Equality_WithDifferentValues_ShouldNotBeEqual()
    {
        var trackingNumber1 = TrackingNumber.Create("VEL-20260830-11111111");
        var trackingNumber2 = TrackingNumber.Create("VEL-20260830-22222222");

        trackingNumber1.Should().NotBe(trackingNumber2);
        (trackingNumber1 == trackingNumber2).Should().BeFalse();
    }
}
