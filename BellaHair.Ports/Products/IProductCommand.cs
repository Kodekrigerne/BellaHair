namespace BellaHair.Ports.Products
{
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
