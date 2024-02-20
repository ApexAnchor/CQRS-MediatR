
using MediatR;

namespace CQRS.MediatR.Domain.Query
{
    public class GetAllProductsQuery : IRequest<List<GetProductsQueryResponse>> 
    {

    }
}
