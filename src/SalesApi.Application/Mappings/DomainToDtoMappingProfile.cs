using AutoMapper;
using SalesApi.Application.DTOs;
using SalesApi.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesApi.Application.Mappings
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            CreateMap<Sale, SaleDto>();
            CreateMap<SaleItem, SaleItemDto>();
            CreateMap<Product, Product>(); // Simples porque no Product não temos DTO ainda
        }
    }
}
