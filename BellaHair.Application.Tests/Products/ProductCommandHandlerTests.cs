using BellaHair.Ports.Products;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Application.Tests.Products
{
    internal sealed class ProductCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public void CreateProduct_Given_ValidProductData_Then_CreatesAndAddsProductToDatabase()
        {
            // Arrange
            var handler = ServiceProvider.GetRequiredService<IProductCommand>();
            var name = "Shampoo";
            var description = "Skælshampoo";
            var price = 39.99m;
            var command = new CreateProductCommand(name, description, price);

            // Act
            handler.CreateProductAsync(command).GetAwaiter().GetResult();

            // Assert
            var productInDb = _db.Products.FirstOrDefault();
            Assert.Multiple(() =>
            {
                Assert.That(productInDb!.Name, Is.EqualTo(name));
                Assert.That(productInDb.Description, Is.EqualTo(description));
                Assert.That(productInDb.Price.Value, Is.EqualTo(price));
            });
        }
    }
}
