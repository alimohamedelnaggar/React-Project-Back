using FluentValidation;
using FoodOrdering.Application.DTOs.Cart;

namespace FoodOrdering.Application.Validators;

public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
{
    public AddToCartDtoValidator()
    {
        RuleFor(x => x.MealId)
            .GreaterThan(0).WithMessage("Invalid meal ID");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");
    }
}

public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
{
    public UpdateCartItemDtoValidator()
    {
        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("Invalid cart item ID");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater")
            .LessThanOrEqualTo(100).WithMessage("Quantity cannot exceed 100");
    }
}
