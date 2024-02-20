using MediatR;

namespace CQRS.MediatR.Domain.Query
{
    public class GetProductByIdQuery : IRequest<GetProductByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
