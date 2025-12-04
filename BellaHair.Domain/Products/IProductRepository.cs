// Mikkel Dahlmann

namespace BellaHair.Domain.Products
{

    /// <summary>
    /// Defines the contract for a repository that manages product entities.
    /// </summary>

    public interface IProductRepository
    {
        Task AddAsync(Product product);
        void Delete(Product product);
        Task<Product> GetAsync(Guid productId);
        Task SaveChangesAsync();
    }
}
