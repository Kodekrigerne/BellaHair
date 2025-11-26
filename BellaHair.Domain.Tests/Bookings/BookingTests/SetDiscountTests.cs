using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using FixtureBuilder;
using Moq;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class SetDiscountTests
    {
        [Test]
        public void Given_UnpaidBooking_Then_DiscountIsSet()
        {
            //Arrange
            var discount = Fixture.New<BookingDiscount>().With(d => d.Name, "Test Discount").Build();
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            var booking = Fixture.New<Booking>().With(b => b.IsPaid, false).Build();

            //Act
            booking.SetDiscount(discount, dateTimeProvider.Object);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(booking.Discount, Is.Not.Null);
                Assert.That(booking.Discount!.Name, Is.EqualTo(discount.Name));
            });
        }

        [Test]
        public void Given_PaidBooking_Then_ThrowsException()
        {
            //Arrange
            var discount = Fixture.New<BookingDiscount>().Build();
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            var booking = Fixture.New<Booking>().With(b => b.IsPaid, true).Build();

            //Act & Assert
            Assert.Throws<BookingException>(() => booking.SetDiscount(discount, dateTimeProvider.Object));
        }
    }
}
