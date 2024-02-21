using CQRS.MediatR.Domain.Command;
using FluentValidation;

namespace CQRS.MediatR.Domain.Validations
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
