using BellaHair.Domain.Bookings;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class TotalTests
    {
        [Test]
        public void TotalBase_Given_TreatmentPrice_Then_ReturnsBasePrice()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.Treatment!.Price.Value, 200m)
                .With(b => b.Discount!.Amount, 50m)
                .Build();

            var total = booking.TotalBase;

            Assert.That(total, Is.EqualTo(200m));
        }

        [Test]
        public void TotalBase_Given_Products_Then_ReturnsBasePrice()
        {
            var productPrice = 200m;
            var quantity = 3;
            var productLine = Fixture.New<ProductLine>()
                .With(pl => pl.Quantity.Value, quantity)
                .With(pl => pl.Product.Price.Value, productPrice)
                .Build();

            var treatmentPrice = 200m;
            var booking = Fixture.New<Booking>()
                .With(b => b.Treatment!.Price.Value, 200m)
                .With(b => b.ProductLines, [productLine])
                .With(b => b.Discount!.Amount, 50m)
                .Build();

            var total = booking.TotalBase;
            var expected = productPrice * quantity + treatmentPrice;

            Assert.That(total, Is.EqualTo(expected));
        }

        [Test]
        public void TotalWithDiscount_Given_TreatmentPriceAndDiscount_Then_ReturnsPriceWithDiscount()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.Treatment!.Price.Value, 200m)
                .With(b => b.Discount!.Amount, 50m)
                .Build();

            var total = booking.TotalWithDiscount;

            Assert.That(total, Is.EqualTo(150m));
        }

        [Test]
        public void TotalWithDiscount_Given_ProductsAndDiscount_Then_ReturnsPriceWithDiscount()
        {
            var productPrice = 200m;
            var quantity = 3;
            var productLine = Fixture.New<ProductLine>()
                .With(pl => pl.Quantity.Value, quantity)
                .With(pl => pl.Product.Price.Value, productPrice)
                .Build();

            var treatmentPrice = 200m;
            var booking = Fixture.New<Booking>()
                .With(b => b.Treatment!.Price.Value, 200m)
                .With(b => b.ProductLines, [productLine])
                .With(b => b.Discount!.Amount, 50m)
                .Build();

            var total = booking.TotalWithDiscount;
            var expected = (productPrice * quantity + treatmentPrice) - booking.Discount!.Amount;

            Assert.That(total, Is.EqualTo(expected));
        }
    }
}
