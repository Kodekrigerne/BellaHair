using BellaHair.Domain.Bookings;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Bookings
{
    public sealed class ProductLineSnapshotTests
    {
        [Test]
        public void Create_Given_ValidParameters_Then_CreatesProductLineSnapshot()
        {
            //Arrange
            var id = Guid.NewGuid();
            var amount = 3;
            var name = "";
            var description = "";
            var price = 200m;

            var productLine = Fixture.New<ProductLine>()
                .With(pl => pl.Id, id)
                .With(pl => pl.Quantity.Value, amount)
                .With(pl => pl.Product.Name, name)
                .With(pl => pl.Product.Description, description)
                .With(pl => pl.Product.Price.Value, price)
                .Build();

            //Act
            var productLineSnapshot = ProductLineSnapshot.FromProductLine(productLine);

            using (Assert.EnterMultipleScope())
            {
                //Assert
                Assert.That(productLineSnapshot.Quantity, Is.EqualTo(amount));
                Assert.That(productLineSnapshot.Name, Is.EqualTo(name));
                Assert.That(productLineSnapshot.Description, Is.EqualTo(description));
                Assert.That(productLineSnapshot.Price, Is.EqualTo(price));
                Assert.That(productLineSnapshot.ProductLineId, Is.EqualTo(id));
            }
        }
    }
}
