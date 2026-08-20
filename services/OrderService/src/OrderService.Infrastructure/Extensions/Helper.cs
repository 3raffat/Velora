using OrderService.Domain.Entities.Customers;

namespace OrderService.Infrastructure.Extensions;

public static class Helper
{
    public static bool IsBirthday(this Customer customer, DateOnly today)
    {
        return customer.IsProfileCompleted
            && customer.DateOfBirth.Month == today.Month
            && customer.DateOfBirth.Day == today.Day;
    }
}
