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
        public void TotalWithDiscount_Given_TreatmentPriceAndDiscount_Then_ReturnsPriceWithDiscount()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.Treatment!.Price.Value, 200m)
                .With(b => b.Discount!.Amount, 50m)
                .Build();

            var total = booking.TotalWithDiscount;

            Assert.That(total, Is.EqualTo(150m));
        }
    }
}
