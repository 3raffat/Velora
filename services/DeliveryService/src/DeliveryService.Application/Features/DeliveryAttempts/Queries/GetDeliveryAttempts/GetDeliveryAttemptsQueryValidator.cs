using FluentValidation;

namespace DeliveryService.Application.Features.DeliveryAttempts.Queries.GetDeliveryAttempts;

public sealed class GetDeliveryAttemptsQueryValidator : AbstractValidator<GetDeliveryAttemptsQuery>
{
    public GetDeliveryAttemptsQueryValidator()
    {
        RuleFor(query => query.ShipmentId).NotEmpty();
    }
}
