using BellaHair.Domain.Bookings;
using FixtureBuilder;
using Moq;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class ValidateDeleteTests
    {
        [Test]
        public void Given_UnpaidFutureBooking_Then_DoesNotThrow()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.DoesNotThrow(() => booking.ValidateDelete(dateTimeProvider.Object));
        }

        [Test]
        public void Given_UnpaidPastBooking_Then_ThrowsException()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(-5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.ValidateDelete(dateTimeProvider.Object));
        }

        [Test]
        public void Given_PaidFutureBooking_Then_ThrowsException()
        {
            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, true)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.ValidateDelete(dateTimeProvider.Object));
        }
    }
}
