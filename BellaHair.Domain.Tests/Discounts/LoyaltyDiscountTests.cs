using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Discounts
{
    internal sealed class LoyaltyDiscountTests
    {
        [TestCase("1")]
        [TestCase("5")]
        public void Create_Given_ValidParameters_Then_ConstructsLoyaltyDiscount(string visitsStr)
        {
            //Arrange
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var discountPercent = DiscountPercent.FromDecimal(0.5m);

            //Act
            var loyaltyDiscount = LoyaltyDiscount.Create(name, minimumVisits, discountPercent);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loyaltyDiscount.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(loyaltyDiscount.Name, Is.EqualTo(name));
                Assert.That(loyaltyDiscount.MinimumVisits, Is.EqualTo(minimumVisits));
                Assert.That(loyaltyDiscount.DiscountPercent.Value, Is.EqualTo(discountPercent.Value));
            }
        }

        [TestCase("-5")]
        [TestCase("0")]
        public void Create_Given_InvalidMinimumVisits_Then_ThrowsException(string visitsStr)
        {
            //Arrange
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var discountPercent = DiscountPercent.FromDecimal(0.5m);

            //Act & Assert
            Assert.Throws<LoyaltyDiscountException>(() => LoyaltyDiscount.Create(name, minimumVisits, discountPercent));
        }

        [Test]
        public void CalculateBookingDiscount_Given_EligibleCustomer_Then_CalculatesDiscount()
        {
            var loyaltyDiscount = Fixture.New<LoyaltyDiscount>()
                .With(l => l.MinimumVisits, 5)
                .With(l => l.DiscountPercent.Value, 0.10m)
                .With(l => l.Name, "Test discount Name")
                .Build();

            var customer = Fixture.New<PrivateCustomer>()
                .With(c => c.Visits, 6)
                .Build();

            var treatment = Fixture.New<Treatment>()
                .With(t => t.Price.Value, 200m)
                .Build();

            var booking = Fixture.New<Booking>()
                .With(b => b.Customer, customer)
                .With(b => b.Treatment, treatment)
                .Build();

            var discount = loyaltyDiscount.CalculateBookingDiscount(booking);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(discount.Amount, Is.EqualTo(200m * 0.10m));
                Assert.That(discount.Type, Is.EqualTo(DiscountType.LoyaltyDiscount));
                Assert.That(discount.Name, Is.EqualTo("Test discount Name"));
                Assert.That(discount.DiscountActive, Is.True);
            }
        }

        [Test]
        public void CalculateBookingDiscount_Given_IneligibleCustomer_Then_CalculatesDiscount()
        {
            var loyaltyDiscount = Fixture.New<LoyaltyDiscount>()
                .With(l => l.MinimumVisits, 5)
                .With(l => l.Name, "Test discount Name")
                .Build();

            var customer = Fixture.New<PrivateCustomer>()
                .With(c => c.Visits, 3)
                .Build();

            var treatment = Fixture.New<Treatment>().Build();

            var booking = Fixture.New<Booking>()
                .With(b => b.Customer, customer)
                .With(b => b.Treatment, treatment)
                .Build();

            var discount = loyaltyDiscount.CalculateBookingDiscount(booking);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(discount.Amount, Is.Zero);
                Assert.That(discount.Type, Is.EqualTo(DiscountType.LoyaltyDiscount));
                Assert.That(discount.Name, Is.EqualTo("Test discount Name"));
                Assert.That(discount.DiscountActive, Is.False);
            }
        }
    }
}
