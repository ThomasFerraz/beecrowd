using AutoMapper;
using MediatR;
using SalesApi.Domain.Entities;
using SalesApi.Application.DTOs;
using SalesApi.Application.Commands.CreateSale;

namespace SalesApi.Application.Commands
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly IMapper _mapper;

        public CreateSaleCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = request.SaleNumber,
                SaleDate = request.SaleDate,
                CustomerId = request.CustomerId,
                BranchId = request.BranchId,
                Cancelled = false,
                Items = new List<SaleItem>()
            };

            foreach (var item in request.Items)
            {
                if (item.Quantity > 20)
                    throw new Exception("You cannot buy more than 20 pieces of same item");

                var discount = 0m;
                if (item.Quantity >= 10)
                    discount = 0.20m;
                else if (item.Quantity >= 4)
                    discount = 0.10m;

                var total = item.UnitPrice * item.Quantity * (1 - discount);

                sale.Items.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = discount,
                    Total = total
                });
            }

            sale.TotalAmount = sale.Items.Sum(i => i.Total);

            // Aqui depois você vai salvar no banco (EF Core)
            // _dbContext.Sales.Add(sale);
            // await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleDto>(sale);
        }
    }
}
