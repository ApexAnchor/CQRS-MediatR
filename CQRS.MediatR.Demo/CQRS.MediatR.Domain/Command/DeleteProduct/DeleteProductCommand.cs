using MediatR;

namespace CQRS.MediatR.Domain.Command
{
    public class DeleteProductCommand : IRequest<DeleteProductCommandResponse>
    {
        public Guid Id { get; set; }
    }
}
