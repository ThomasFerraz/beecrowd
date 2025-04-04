using Microsoft.AspNetCore.Mvc;
using MediatR;
using SalesApi.Application.Commands.CreateSale;
using SalesApi.Application.DTOs;
using SalesApi.Domain.Entities;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(Guid id, [FromBody] UpdateSaleCommand command)
        {
            var sale = SalesControllerMemory.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                return NotFound(new
                {
                    data = null as object,
                    status = "error",
                    message = "Venda não encontrada"
                });
            }

            sale.SaleNumber = command.SaleNumber ?? sale.SaleNumber;
            sale.SaleDate = command.SaleDate ?? sale.SaleDate;

            return Ok(new
            {
                data = sale,
                status = "success",
                message = "Venda atualizada com sucesso"
            });
        }

        [HttpGet]
        public IActionResult GetAllSales()
        {
            var sales = SalesControllerMemory.Sales;

            return Ok(new
            {
                data = sales,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            var result = await _mediator.Send(command);

            Console.WriteLine($"[EVENTO] SaleCreated: Venda {result.Id} criada.");

            return Ok(new
            {
                data = result,
                status = "success",
                message = "Venda criada com sucesso"
            });
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            // Este exemplo simula uma lista em memória
            // No mundo real, você buscaria no banco de dados
            var sale = SalesControllerMemory.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                return NotFound(new
                {
                    data = null as object,
                    status = "error",
                    message = "Venda não encontrada"
                });
            }

            sale.Cancelled = true;

            Console.WriteLine($"[EVENTO] SaleCancelled: Venda {sale.Id} cancelada.");

            return Ok(new
            {
                data = sale,
                status = "success",
                message = "Venda cancelada com sucesso"
            });
        }

    }
}
