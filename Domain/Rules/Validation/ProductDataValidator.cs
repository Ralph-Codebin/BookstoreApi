using Domain.Model.Entities;
using Domain.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using static Domain.Rules.Validation.ProductDataValidator;

namespace Domain.Rules.Validation
{
    public class ProductDataValidator : AbstractValidator<DNewProductRequest>
    {
        public ProductDataValidator()
        {
            RuleFor(r => r.title).NotEmpty().MaximumLength(50);
            RuleFor(r => r.description).NotNull().MaximumLength(500).WithMessage("Please give a short description of the product");
            RuleFor(r => r.imagePath).NotNull().MaximumLength(100);
            RuleFor(r => r.price).NotNull();
        }
    }

    public class BusinessDataUpdateValidator : AbstractValidator<DUpdateProductDataRequest>
    {
        public BusinessDataUpdateValidator()
        {
            RuleFor(r => r.id).NotEmpty();
            RuleFor(r => r.title).NotEmpty().MaximumLength(50);
            RuleFor(r => r.description).NotNull().MaximumLength(500).WithMessage("Please give a short description of the product");
            RuleFor(r => r.imagePath).NotNull().MaximumLength(100);
            RuleFor(r => r.price).NotNull();
        }
    }
}