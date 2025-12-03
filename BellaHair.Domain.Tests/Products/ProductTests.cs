using BellaHair.Domain.Products;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.Products
{
    internal sealed class ProductTests
    {
        [Test]
        public void CreateProduct_Given_ValidInputs_Then_CreatesProduct()
        {
            // Arrange
            var name = "Shampoo";
            var price = Price.FromDecimal(99.99m);
            var description = "Skælshampoo";

            // Act
            var product = Product.Create(name, description, price);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(product.Name, Is.EqualTo(name));
                Assert.That(product.Description, Is.EqualTo(description));
                Assert.That(product.Price, Is.EqualTo(price));
            });
        }
    }
}
