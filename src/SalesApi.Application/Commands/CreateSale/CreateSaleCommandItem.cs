namespace SalesApi.Application.Commands.CreateSale
{
    public class CreateSaleCommandItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
