using bookstore_api.Requests;
using Domain.Model.Entities;
using Domain.Services;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using static Domain.Rules.Validation.ProductDataValidator;

namespace Domain.Rules.Validation
{
    public class SubscriptionDataValidator : AbstractValidator<DNewSubscriptiontRequest>
    {
        public SubscriptionDataValidator()
        {
            RuleFor(r => r.prodid).NotEmpty();
            RuleFor(r => r.email).NotNull().MaximumLength(50);
            RuleFor(r => r.name).NotNull().MaximumLength(50);
        }
    }

    public class UpdateSubscriptiontValidator : AbstractValidator<DUpdateSubscriptiontRequest>
    {
        public UpdateSubscriptiontValidator()
        {
            RuleFor(r => r.id).NotEmpty();
            RuleFor(r => r.state).NotNull();
        }
    }
}