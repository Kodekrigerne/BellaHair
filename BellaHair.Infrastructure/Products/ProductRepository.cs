using BellaHair.Domain.Products;

namespace BellaHair.Infrastructure.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly BellaHairContext _db;

        public ProductRepository(BellaHairContext db) => _db = db;

        async Task IProductRepository.AddAsync(Product product)
        {
            await _db.Products.AddAsync(product);
        }

        void IProductRepository.Delete(Product product)
        {
            _db.Products.Remove(product);
        }

        async Task<Product> IProductRepository.GetAsync(Guid productId)
        {
            var product = await _db.Products.FindAsync(productId) ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");
            return product;
        }

        async Task IProductRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
