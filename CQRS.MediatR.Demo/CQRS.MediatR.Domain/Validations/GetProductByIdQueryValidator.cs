
using CQRS.MediatR.Domain.Query;
using FluentValidation;

namespace CQRS.MediatR.Domain.Validations
{
    public class GetProductByIdQueryValidator :  AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
