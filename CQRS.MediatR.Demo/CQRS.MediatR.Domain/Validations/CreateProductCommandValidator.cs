using CQRS.MediatR.Domain.Command;
using FluentValidation;

namespace CQRS.MediatR.Domain.Validations
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Description).NotEmpty();
            RuleFor(x=>x.UnitPrice).GreaterThanOrEqualTo(1);
            RuleFor(x=>x.Quantity).GreaterThanOrEqualTo(10);
        }
    }
}
