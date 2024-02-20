
using AutoMapper;
using CQRS.MediatR.Data;
using MediatR;

namespace CQRS.MediatR.Domain.Command
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
    {
        private readonly ProductDbContext productDbContext;
        private readonly IMapper mapper;

        public CreateProductCommandHandler(ProductDbContext productDbContext,IMapper mapper)
        {
            this.productDbContext = productDbContext;
            this.mapper = mapper;
        }
        public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);
            product.Id = Guid.NewGuid();
            await productDbContext.Products.AddAsync(product);
            await productDbContext.SaveChangesAsync();
            var response = mapper.Map<CreateProductCommandResponse>(product);
            return response;
        }
    }
}
