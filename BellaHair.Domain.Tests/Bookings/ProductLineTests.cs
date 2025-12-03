using BellaHair.Domain.Bookings;
using BellaHair.Domain.Products;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Bookings
{
    public sealed class ProductLineTests
    {
        [Test]
        public void Create_Given_ValidParameters_Then_CreatesProductLine()
        {
            var amount = 3;
            var name = "";
            var description = "";
            var price = 200m;

            //Arrange
            var quantity = Fixture.New<Quantity>().With(q => q.Value, amount).Build();
            var product = Fixture.New<Product>()
                .With(p => p.Name, name)
                .With(p => p.Description, description)
                .With(p => p.Price.Value, price)
                .Build();

            //Act
            var productLine = ProductLine.Create(quantity, product);

            //Assert
            Assert.That(productLine.Quantity.Value, Is.EqualTo(amount));
            Assert.That(productLine.Product.Name, Is.EqualTo(name));
            Assert.That(productLine.Product.Description, Is.EqualTo(description));
            Assert.That(productLine.Product.Price.Value, Is.EqualTo(price));
            Assert.That(productLine.Id, Is.Not.EqualTo(Guid.Empty));
        }
    }
}
