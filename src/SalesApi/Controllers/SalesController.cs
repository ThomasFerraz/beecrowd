using Microsoft.AspNetCore.Mvc;
using MediatR;
using SalesApi.Application.Commands.CreateSale;
using SalesApi.Application.DTOs;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Repositories;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISaleRepository _saleRepository;

        public SalesController(IMediator mediator, ISaleRepository saleRepository)
        {
            _mediator = mediator;
            _saleRepository = saleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);

            var sale = new Sale
            {
                Id = result.Id,
                SaleNumber = result.SaleNumber,
                SaleDate = result.SaleDate,
                CustomerId = result.CustomerId,
                BranchId = result.BranchId,
                TotalAmount = result.TotalAmount,
                Cancelled = result.Cancelled,
                Items = result.Items.Select(i => new SaleItem
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Total = i.Total,
                    IsCancelled = i.IsCancelled
                }).ToList()
            };

            await _saleRepository.AddAsync(sale);

            Console.WriteLine($"[EVENTO] SaleCreated: Venda {result.Id} criada.");

            return Ok(new
            {
                data = result,
                status = "success",
                message = "Venda criada com sucesso"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleRepository.GetAllAsync();

            return Ok(new
            {
                data = sales,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            var sale = await _saleRepository.GetByIdAsync(id);

            if (sale == null)
            {
                return NotFound(new
                {
                    data = (object)null,
                    status = "error",
                    message = "Venda não encontrada"
                });
            }

            await _saleRepository.CancelAsync(id);

            Console.WriteLine($"[EVENTO] SaleCancelled: Venda {sale.Id} cancelada.");

            return Ok(new
            {
                data = sale,
                status = "success",
                message = "Venda cancelada com sucesso"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleCommand command)
        {
            var sale = await _saleRepository.GetByIdAsync(id);

            if (sale == null)
            {
                return NotFound(new
                {
                    data = (object)null,
                    status = "error",
                    message = "Venda não encontrada"
                });
            }

            sale.SaleNumber = command.SaleNumber ?? sale.SaleNumber;
            sale.SaleDate = command.SaleDate ?? sale.SaleDate;

            Console.WriteLine($"[EVENTO] SaleUpdated: Venda {sale.Id} atualizada.");

            return Ok(new
            {
                data = sale,
                status = "success",
                message = "Venda atualizada com sucesso"
            });
        }
    }
}
