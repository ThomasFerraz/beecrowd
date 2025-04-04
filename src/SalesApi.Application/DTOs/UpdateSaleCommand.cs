namespace SalesApi.Application.DTOs
{
    public class UpdateSaleCommand
    {
        public string? SaleNumber { get; set; }
        public DateTime? SaleDate { get; set; }
    }
}
