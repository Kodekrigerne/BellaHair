// Dennis & Mikkel Dahlmann

namespace BellaHair.Ports.Products
{

    /// <summary>
    /// Defines methods for creating, updating, and deleting products asynchronously.
    /// </summary>

    public interface IProductCommand
    {
        Task CreateProductAsync(CreateProductCommand command);
        Task DeleteProductAsync(DeleteProductCommand command);
        Task UpdateProductAsync(UpdateProductCommand command);
    }

    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price);

    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price);

    public record DeleteProductCommand(Guid Id);
}
