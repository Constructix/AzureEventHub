using FluentValidation;

namespace EventTopicDemo.Validation;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleForEach(x => x.SubOrders)
            .Must(y => y.IsOrderPickUp && y.DeliveryAddress != null)
            .WithMessage("Delivery Address mus be supplied if OrderPickup is true.");
    }
}