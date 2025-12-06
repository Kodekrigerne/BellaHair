using BellaHair.Domain.Products;
using BellaHair.Domain.SharedValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure.Tests.Products
{
    internal sealed class ProductRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void AddAsync_Given_ValidProduct_Then_AddsProductToDatabase()
        {
            // Arrange
            var repo = ServiceProvider.GetRequiredService<IProductRepository>();
            var name = "Shampoo";
            var description = "Skælshampoo";
            var price = Price.FromDecimal(49.99m);
            var product = Product.Create(name, description, price);

            // Act
            repo.AddAsync(product).GetAwaiter().GetResult();

            // Assert
            repo.SaveChangesAsync().GetAwaiter().GetResult();
            var addedProduct = _db.Products.FirstOrDefault();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(addedProduct!.Name, Is.EqualTo(name));
                Assert.That(addedProduct.Description, Is.EqualTo(description));
                Assert.That(addedProduct.Price.Value, Is.EqualTo(price.Value));
            }
        }
    }
}
