using SalesApi.Domain.Entities;

namespace SalesApi.Infrastructure.Repositories
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(Guid id);
        Task AddAsync(Sale sale);
        Task CancelAsync(Guid id);
    }
}
