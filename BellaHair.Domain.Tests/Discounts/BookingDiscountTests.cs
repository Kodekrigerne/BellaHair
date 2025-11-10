using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Tests.Discounts
{
    //Dennis
    internal sealed class BookingDiscountTests
    {
        [Test]
        public void Given_ActiveFactory_Then_ConstructsActiveBookingDiscount()
        {
            var name = "Test Name";
            var discountAmount = 5.25m;

            var bookingDiscount = BookingDiscount.Active(name, discountAmount);

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
            var name = "Test Name";

            var bookingDiscount = BookingDiscount.Inactive(name);

            Assert.Multiple(() =>
            {
                Assert.That(bookingDiscount.Name, Is.EqualTo(name));
                Assert.That(bookingDiscount.DiscountActive, Is.False);
            });
        }
    }
}
