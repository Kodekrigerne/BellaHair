using BellaHair.Ports.Products;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Products
{
    public class ProductQueryHandler : IProductQuery
    {
        private readonly BellaHairContext _db;

        public ProductQueryHandler(BellaHairContext db) => _db = db;

        async Task<IEnumerable<ProductDTO>> IProductQuery.GetAllAsync()
        {
            return await _db.Products
                .AsNoTracking()
                .Select(p => new ProductDTO(p.Id, p.Name, p.Description, p.Price.Value))
                .ToListAsync();
        }
    }
}
