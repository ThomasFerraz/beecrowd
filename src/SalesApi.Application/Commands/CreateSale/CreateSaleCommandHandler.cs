using AutoMapper;
using MediatR;
using SalesApi.Domain.Entities;
using SalesApi.Application.DTOs;
using SalesApi.Application.Commands.CreateSale;
using SalesApi.Infrastructure.Repositories;
using SalesApi.Application.Exceptions;

namespace SalesApi.Application.Commands
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public CreateSaleCommandHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
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
                    throw new InvalidSaleException("You cannot buy more than 20 pieces of same item");

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidSaleException($"Produto com ID {item.ProductId} não encontrado.");

                if (product.Price != item.UnitPrice)
                    throw new InvalidSaleException($"Price mismatch: Expected {product.Price}, received {item.UnitPrice}");

                var unitPrice = product.Price;

                var discount = 0m;
                if (item.Quantity >= 10)
                    discount = 0.20m;
                else if (item.Quantity >= 4)
                    discount = 0.10m;

                var total = unitPrice * item.Quantity * (1 - discount);

                sale.Items.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    Discount = discount,
                    Total = total
                });
            }

            sale.TotalAmount = sale.Items.Sum(i => i.Total);

            return _mapper.Map<SaleDto>(sale);
        }
    }
}
