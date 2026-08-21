namespace DeliveryService.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? GetUserId();
}
