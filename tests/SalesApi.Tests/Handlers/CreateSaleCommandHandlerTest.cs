using AutoMapper;
using Bogus;
using SalesApi.Application.Commands;
using SalesApi.Application.DTOs;
using SalesApi.Application.Mappings;

using Xunit;

namespace SalesApi.Tests.Handlers
{
    public class CreateSaleCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Faker _faker;

        public CreateSaleCommandHandlerTest()
        {
            // Configurando AutoMapper manualmente para os testes
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToDtoMappingProfile>();
            });
            _mapper = configuration.CreateMapper();

            // Instanciando o Faker
            _faker = new Faker();
        }

        [Fact(DisplayName = "Deve criar venda com desconto de 10% para 4 itens")]
        public async Task Deve_Criar_Venda_Com_Desconto_10_Porcento()
        {
            // Arrange (Preparar o cenário)
            var command = new Application.Commands.CreateSale.CreateSaleCommand
            {
                SaleNumber = _faker.Random.Number(1000, 9999).ToString(),
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                Items = new List<CreateSaleItemDto>
                {
                    new CreateSaleItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 4,         // Aqui testamos o desconto de 10%
                        UnitPrice = 100m
                    }
                }
            };

            var handler = new CreateSaleCommandHandler(_mapper);

            // Act (Executar a ação)
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert (Validar o resultado)
            Assert.NotNull(result);
            Assert.Equal(command.SaleNumber, result.SaleNumber);
            Assert.Single(result.Items);
            Assert.Equal(4, result.Items.First().Quantity);
            Assert.Equal(0.10m, result.Items.First().Discount); // 10% de desconto
            Assert.Equal(360m, result.Items.First().Total);     // 400 - 10% = 360
        }
    }
}
