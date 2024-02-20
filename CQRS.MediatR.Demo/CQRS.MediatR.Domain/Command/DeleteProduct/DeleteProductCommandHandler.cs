using CQRS.MediatR.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.MediatR.Domain.Command
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
    {
        private readonly ProductDbContext productDbContext;

        public DeleteProductCommandHandler(ProductDbContext productDbContext)
        {
            this.productDbContext = productDbContext;
        }
        public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var prd = await productDbContext.Products.FindAsync(request.Id);
            if (prd is null)
            {
                return new DeleteProductCommandResponse() { IsDeleteSuccessful = false };
            }
            productDbContext.Products.Remove(prd);
            await productDbContext.SaveChangesAsync();            
            return new DeleteProductCommandResponse() { IsDeleteSuccessful = true };
        }
    }
}
