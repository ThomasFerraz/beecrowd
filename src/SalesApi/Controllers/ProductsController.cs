using Microsoft.AspNetCore.Mvc;
using SalesApi.Domain.Entities;
using SalesApi.Infrastructure.Repositories;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(new
            {
                data = products,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            product.Id = Guid.NewGuid();

            await _productRepository.AddAsync(product);

            Console.WriteLine($"[EVENTO] ProductCreated: Produto {product.Id} criado - {product.Title}.");

            return Ok(new
            {
                data = product,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }
    }
}
