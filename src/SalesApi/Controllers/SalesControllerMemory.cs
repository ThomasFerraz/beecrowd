using SalesApi.Application.DTOs;

namespace SalesApi.Controllers
{
    public static class SalesControllerMemory
    {
        public static List<SaleDto> Sales { get; set; } = new List<SaleDto>();
    }
}
