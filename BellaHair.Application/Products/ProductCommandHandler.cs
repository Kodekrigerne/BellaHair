using BellaHair.Domain.Products;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Ports.Products;

namespace BellaHair.Application.Products
{
    public class ProductCommandHandler : IProductCommand
    {
        private readonly IProductRepository _productRepository;

        public ProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        async Task IProductCommand.CreateProductAsync(CreateProductCommand command)
        {
            var price = Price.FromDecimal(command.Price);
            var product = Product.Create(command.Name, command.Description, price);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        Task IProductCommand.DeleteProductAsync(DeleteProductCommand command)
        {
            throw new NotImplementedException();
        }

        Task IProductCommand.UpdateProductAsync(UpdateProductCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
