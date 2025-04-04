using Microsoft.AspNetCore.Mvc;
using SalesApi.Domain.Entities;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                data = Products,
                status = "success",
                message = "Operação concluída com sucesso"
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            product.Id = Guid.NewGuid();
            
            Products.Add(product);
            
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
