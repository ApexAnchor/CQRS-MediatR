using MediatR;

namespace CQRS.MediatR.Domain.Command
{
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {       
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        
    }
}
