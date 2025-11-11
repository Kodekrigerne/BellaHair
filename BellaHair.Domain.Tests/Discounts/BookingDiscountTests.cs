using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Tests.Discounts
{
    //Dennis
    internal sealed class BookingDiscountTests
    {
        [Test]
        public void Given_ActiveFactory_Then_ConstructsActiveBookingDiscount()
        {
            //Arrange
            var name = "Test Name";
            var discountAmount = 5.25m;

            //Act
            var bookingDiscount = BookingDiscount.Active(name, discountAmount);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(bookingDiscount.Name, Is.EqualTo(name));
                Assert.That(bookingDiscount.Amount, Is.EqualTo(discountAmount));
                Assert.That(bookingDiscount.DiscountActive, Is.True);
            });
        }

        [Test]
        public void Given_InactiveFactory_Then_ConstructsInActiveBookingDiscount()
        {
            //Arrange
            var name = "Test Name";

            //Act
            var bookingDiscount = BookingDiscount.Inactive(name);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(bookingDiscount.Name, Is.EqualTo(name));
                Assert.That(bookingDiscount.DiscountActive, Is.False);
            });
        }
    }
}
