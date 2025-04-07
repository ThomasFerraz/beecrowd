using SalesApi.Domain.Entities;

namespace SalesApi.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private static readonly List<Sale> Sales = new List<Sale>();

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await Task.FromResult(Sales);
        }

        public async Task<Sale?> GetByIdAsync(Guid id)
        {
            return await Task.FromResult(Sales.FirstOrDefault(x => x.Id == id));
        }

        public async Task AddAsync(Sale sale)
        {
            Sales.Add(sale);
            await Task.CompletedTask;
        }

        public async Task CancelAsync(Guid id)
        {
            var sale = Sales.FirstOrDefault(x => x.Id == id);
            if (sale != null)
                sale.Cancelled = true;

            await Task.CompletedTask;
        }
    }
}
