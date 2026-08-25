using FluentValidation;
using FunPortal.Application.DTOs.Products;
using FunPortal.Domain.Enums;

namespace FunPortal.Application.Validators.Products;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("Invalid product type");

        // Book-specific validation
        When(x => x.ProductType == ProductType.PhysicalBook, () =>
        {
            RuleFor(x => x.Author)
                .MaximumLength(200).WithMessage("Author must not exceed 200 characters");

            RuleFor(x => x.ISBN)
                .MaximumLength(20).WithMessage("ISBN must not exceed 20 characters");
        });

        // Video-specific validation
        When(x => x.ProductType == ProductType.Video, () =>
        {
            RuleFor(x => x.Director)
                .MaximumLength(200).WithMessage("Director must not exceed 200 characters");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0")
                .When(x => x.DurationMinutes.HasValue);
        });

        // Membership-specific validation
        When(x => x.ProductType == ProductType.Membership, () =>
        {
            RuleFor(x => x.MembershipType)
                .NotNull().WithMessage("Membership type is required for membership products")
                .IsInEnum().WithMessage("Invalid membership type");

            RuleFor(x => x.DurationMonths)
                .NotNull().WithMessage("Duration is required for membership products")
                .GreaterThan(0).WithMessage("Duration must be greater than 0");
        });
    }
}

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Author)
            .MaximumLength(200).WithMessage("Author must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Author));

        RuleFor(x => x.ISBN)
            .MaximumLength(20).WithMessage("ISBN must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.ISBN));

        RuleFor(x => x.Director)
            .MaximumLength(200).WithMessage("Director must not exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Director));

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .When(x => x.DurationMinutes.HasValue);
    }
}
