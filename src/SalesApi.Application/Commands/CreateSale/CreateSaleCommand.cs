using MediatR;
using SalesApi.Application.DTOs;

namespace SalesApi.Application.Commands.CreateSale
{
    public class CreateSaleCommand : IRequest<SaleDto>
    {
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BranchId { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }
}
