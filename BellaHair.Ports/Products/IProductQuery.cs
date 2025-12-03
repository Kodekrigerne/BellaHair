namespace BellaHair.Ports.Products
{
    public interface IProductQuery
    {

    }

    public record ProductDTO(Guid Id, string Name, string Description, decimal Price);
}
