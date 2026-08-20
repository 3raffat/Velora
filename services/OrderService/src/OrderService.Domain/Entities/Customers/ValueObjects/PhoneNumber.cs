using PhoneNumbers;
using OrderService.Domain.Entities.Customers.Exceptions;

namespace OrderService.Domain.Entities.Customers.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string rawNumber, string region = "JO")
    {
        if (string.IsNullOrWhiteSpace(rawNumber))
            throw new InvalidPhoneNumberException(rawNumber);

        var util = PhoneNumberUtil.GetInstance();

        try
        {
            var parsed = util.Parse(rawNumber, region);

            if (!util.IsValidNumber(parsed))
                throw new InvalidPhoneNumberException(rawNumber);

            var formatted = util.Format(parsed, PhoneNumberFormat.E164);

            return new PhoneNumber(formatted);
        }
        catch (NumberParseException)
        {
            throw new InvalidPhoneNumberException(rawNumber);
        }
    }
};
