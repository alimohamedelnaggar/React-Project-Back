using FluentValidation;
using FoodOrdering.Application.DTOs.Meal;

namespace FoodOrdering.Application.Validators;

public class CreateMealDtoValidator : AbstractValidator<CreateMealDto>
{
    public CreateMealDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Meal name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(10000).WithMessage("Price must be less than 10000");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Invalid category ID");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters");
    }
}

public class UpdateMealDtoValidator : AbstractValidator<UpdateMealDto>
{
    public UpdateMealDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid meal ID");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Meal name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(10000).WithMessage("Price must be less than 10000");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Invalid category ID");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters");
    }
}
