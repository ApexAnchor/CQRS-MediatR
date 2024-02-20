using AutoMapper;
using CQRS.MediatR.Data;
using MediatR;

namespace CQRS.MediatR.Domain.Query
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdQueryResponse>
    {
        private readonly ProductDbContext productDbContext;
        private readonly IMapper mapper;

        public GetProductByIdHandler(ProductDbContext productDbContext,IMapper mapper)
        {
            this.productDbContext = productDbContext;
            this.mapper = mapper;
        }
        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await productDbContext.Products.FindAsync(request.Id, cancellationToken);

            return mapper.Map<GetProductByIdQueryResponse>(response);            
        }
    }
}
