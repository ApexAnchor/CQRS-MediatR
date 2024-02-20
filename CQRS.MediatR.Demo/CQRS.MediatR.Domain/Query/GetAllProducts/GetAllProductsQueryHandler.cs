
using AutoMapper;
using CQRS.MediatR.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.MediatR.Domain.Query
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetProductsQueryResponse>>
    {
        private readonly ProductDbContext productDbContext;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(ProductDbContext productDbContext, IMapper mapper)
        {
            this.productDbContext = productDbContext;
            this.mapper = mapper;
        }
        public async Task<List<GetProductsQueryResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
           var response = await productDbContext.Products.ToListAsync(cancellationToken).ConfigureAwait(false);
           var result = mapper.Map<List<GetProductsQueryResponse>>(response); 
           return result;           
        }
    }
}
