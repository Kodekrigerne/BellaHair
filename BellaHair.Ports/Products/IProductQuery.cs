namespace BellaHair.Ports.Products
{
    public interface IProductQuery
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
    }

    public record ProductDTO(Guid Id, string Name, string Description, decimal Price);
}
