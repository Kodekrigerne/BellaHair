// Dennis

namespace BellaHair.Ports.Products
{

    /// <summary>
    /// Represents a query service for retrieving product data.
    /// </summary>

    public interface IProductQuery
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
    }

    public record ProductDTO(Guid Id, string Name, string Description, decimal Price);
}
