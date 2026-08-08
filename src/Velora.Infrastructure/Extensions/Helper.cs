using Velora.Domain.Entities.Customers;

namespace Velora.Infrastructure.Extensions;

public static class Helper
{
    public static bool IsBirthday(this Customer customer, DateOnly today)
    {
        return customer.IsProfileCompleted
            && customer.DateOfBirth.Month == today.Month
            && customer.DateOfBirth.Day == today.Day;
    }
}
