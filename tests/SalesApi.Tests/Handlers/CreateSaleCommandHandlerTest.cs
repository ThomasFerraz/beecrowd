using Xunit;
using SalesApi.Application.Commands;
using SalesApi.Application.Commands.CreateSale;
using SalesApi.Infrastructure.Repositories;
using AutoMapper;
using NSubstitute;
using System.Threading.Tasks;
using System.Collections.Generic;
using SalesApi.Application.DTOs;
using System;

namespace SalesApi.Tests
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public CreateSaleCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SalesApi.Application.Mappings.DomainToDtoMappingProfile());
            });
            _mapper = config.CreateMapper();
            _productRepository = Substitute.For<IProductRepository>();
        }

        [Fact]
        public async Task Should_Create_Sale_When_Data_Is_Valid()
        {
            // Arrange
            var handler = new CreateSaleCommandHandler(_mapper, _productRepository);

            var command = new CreateSaleCommand
            {
                SaleNumber = "1234",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleCommandItem>
                {
                    new CreateSaleCommandItem
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 5.00m
                    }
                }
            };

            _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(new SalesApi.Domain.Entities.Product
            {
                Id = command.Items[0].ProductId,
                Price = 5.00m
            });

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Items.Count);
        }
    }
}
