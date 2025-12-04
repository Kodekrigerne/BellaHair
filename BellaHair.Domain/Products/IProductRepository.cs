namespace BellaHair.Domain.Products
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid productId);
    }
}
