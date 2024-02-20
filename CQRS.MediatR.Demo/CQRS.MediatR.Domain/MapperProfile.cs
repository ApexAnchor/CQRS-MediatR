using AutoMapper;
using CQRS.MediatR.Data;
using CQRS.MediatR.Domain.Command;
using CQRS.MediatR.Domain.Query;

namespace CQRS.MediatR.Domain
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, GetProductsQueryResponse>();

            CreateMap<Product,GetProductByIdQueryResponse>();

            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, CreateProductCommandResponse>().ReverseMap();

        }
    }
}
