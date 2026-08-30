using DeliveryService.Domain.Common.Exceptions;
using DeliveryService.Domain.Common.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DeliveryService.Domain.UnitTests.ValueObjects;

public class AddressSnapshotTests
{
    private const string ValidAddressLine1 = "Zahran Street";
    private const string ValidAddressLine2 = "Building 12, Apt 4";
    private const string ValidCity = "Amman";
    private const string ValidState = "Amman Governorate";
    private const string ValidCountry = "Jordan";

    [Fact]
    public void Create_WithValidParameters_ShouldSetAllProperties()
    {
        var address = AddressSnapshot.Create(
            ValidAddressLine1,
            ValidAddressLine2,
            ValidCity,
            ValidState,
            ValidCountry
        );

        address.Should().NotBeNull();
        address.AddressLine1.Should().Be(ValidAddressLine1);
        address.AddressLine2.Should().Be(ValidAddressLine2);
        address.City.Should().Be(ValidCity);
        address.State.Should().Be(ValidState);
        address.Country.Should().Be(ValidCountry);
    }

    [Fact]
    public void Create_WithTrimmedValues_ShouldTrimAllFields()
    {
        var address = AddressSnapshot.Create(
            "  Zahran Street  ",
            "  Building 12, Apt 4  ",
            "  Amman  ",
            "  Amman Governorate  ",
            "  Jordan  "
        );

        address.AddressLine1.Should().Be("Zahran Street");
        address.AddressLine2.Should().Be("Building 12, Apt 4");
        address.City.Should().Be("Amman");
        address.State.Should().Be("Amman Governorate");
        address.Country.Should().Be("Jordan");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceAddressLine2_ShouldSetAddressLine2ToNull(
        string? addressLine2
    )
    {
        var address = AddressSnapshot.Create(
            ValidAddressLine1,
            addressLine2,
            ValidCity,
            ValidState,
            ValidCountry
        );

        address.AddressLine2.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceAddressLine1_ShouldThrowRequiredFieldException(
        string? invalidValue
    )
    {
        var act = () =>
            AddressSnapshot.Create(
                invalidValue!,
                ValidAddressLine2,
                ValidCity,
                ValidState,
                ValidCountry
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*addressLine1*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceCity_ShouldThrowRequiredFieldException(
        string? invalidValue
    )
    {
        var act = () =>
            AddressSnapshot.Create(
                ValidAddressLine1,
                ValidAddressLine2,
                invalidValue!,
                ValidState,
                ValidCountry
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*city*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceState_ShouldThrowRequiredFieldException(
        string? invalidValue
    )
    {
        var act = () =>
            AddressSnapshot.Create(
                ValidAddressLine1,
                ValidAddressLine2,
                ValidCity,
                invalidValue!,
                ValidCountry
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*state*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrWhitespaceCountry_ShouldThrowRequiredFieldException(
        string? invalidValue
    )
    {
        var act = () =>
            AddressSnapshot.Create(
                ValidAddressLine1,
                ValidAddressLine2,
                ValidCity,
                ValidState,
                invalidValue!
            );

        act.Should().Throw<RequiredFieldException>().WithMessage("*country*");
    }

    [Fact]
    public void Equality_WithSameValues_ShouldBeEqual()
    {
        var address1 = AddressSnapshot.Create(
            "Zahran Street",
            "Apt 4",
            "Amman",
            "Amman Governorate",
            "Jordan"
        );
        var address2 = AddressSnapshot.Create(
            "Zahran Street",
            "Apt 4",
            "Amman",
            "Amman Governorate",
            "Jordan"
        );

        address1.Should().Be(address2);
        (address1 == address2).Should().BeTrue();
    }

    [Fact]
    public void Equality_WithDifferentValues_ShouldNotBeEqual()
    {
        var address1 = AddressSnapshot.Create(
            "Zahran Street",
            "Apt 4",
            "Amman",
            "Amman Governorate",
            "Jordan"
        );
        var address2 = AddressSnapshot.Create(
            "Baghdad Street",
            "Apt 2",
            "Irbid",
            "Irbid Governorate",
            "Jordan"
        );

        address1.Should().NotBe(address2);
        (address1 == address2).Should().BeFalse();
    }
}
