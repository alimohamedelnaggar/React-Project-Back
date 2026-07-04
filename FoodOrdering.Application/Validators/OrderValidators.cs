using FluentValidation;
using FoodOrdering.Application.DTOs.Order;

namespace FoodOrdering.Application.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.ShippingAddress)
            .NotEmpty().WithMessage("Shipping address is required")
            .MaximumLength(500).WithMessage("Shipping address must not exceed 500 characters");
    }
}
