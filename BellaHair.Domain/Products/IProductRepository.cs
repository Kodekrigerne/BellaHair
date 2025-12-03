namespace BellaHair.Domain.Products
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        void Delete(Product product);
        Task<Product> GetAsync(Guid productId);
        Task SaveChangesAsync();
    }
}
