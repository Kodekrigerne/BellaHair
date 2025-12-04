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
                Assert.That(loyaltyDiscount.TreatmentDiscountPercent.Value, Is.EqualTo(discountPercent.Value));
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

        [TestCase("1")]
        [TestCase("5")]
        public void CreateWithProductDiscount_Given_ValidParameters_Then_ConstructsLoyaltyDiscount(string visitsStr)
        {
            //Arrange
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var treatmentDiscountPercent = DiscountPercent.FromDecimal(0.5m);
            var productDiscountPercent = DiscountPercent.FromDecimal(0.25m);

            //Act
            var loyaltyDiscount = LoyaltyDiscount.CreateWithProductDiscount(name, minimumVisits, treatmentDiscountPercent, productDiscountPercent);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(loyaltyDiscount.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(loyaltyDiscount.Name, Is.EqualTo(name));
                Assert.That(loyaltyDiscount.MinimumVisits, Is.EqualTo(minimumVisits));
                Assert.That(loyaltyDiscount.TreatmentDiscountPercent.Value, Is.EqualTo(treatmentDiscountPercent.Value));
                Assert.That(loyaltyDiscount.ProductDiscountPercent, Is.Not.Null);
                Assert.That(loyaltyDiscount.ProductDiscountPercent!.Value, Is.EqualTo(productDiscountPercent.Value));
            }
        }

        [TestCase("-5")]
        [TestCase("0")]
        public void CreateWithProductDiscount_Given_InvalidMinimumVisits_Then_ThrowsException(string visitsStr)
        {
            //Arrange
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var treatmentDiscountPercent = DiscountPercent.FromDecimal(0.5m);
            var productDiscountPercent = DiscountPercent.FromDecimal(0.25m);

            //Act & Assert
            Assert.Throws<LoyaltyDiscountException>(() => LoyaltyDiscount.CreateWithProductDiscount(name, minimumVisits, treatmentDiscountPercent, productDiscountPercent));
        }

        [Test]
        public void CalculateBookingDiscount_Given_EligibleCustomer_Then_CalculatesDiscount()
        {
            //Arrange
            var treatmentDiscount = 0.10m;
            var price = 200m;

            var loyaltyDiscount = Fixture.New<LoyaltyDiscount>()
                .With(l => l.MinimumVisits, 5)
                .With(l => l.TreatmentDiscountPercent.Value, treatmentDiscount)
                .With(l => l.Name, "Test discount Name")
                .Build();

            var booking = Fixture.New<Booking>()
                .With(b => b.Customer!.Visits, 6)
                .With(b => b.Treatment!.Price.Value, price)
                .Build();

            //Act
            var discount = loyaltyDiscount.CalculateBookingDiscount(booking);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(discount.Amount, Is.EqualTo(price * treatmentDiscount));
                Assert.That(discount.Type, Is.EqualTo(DiscountType.LoyaltyDiscount));
                Assert.That(discount.Name, Is.EqualTo(loyaltyDiscount.Name));
                Assert.That(discount.DiscountActive, Is.True);
            }
        }

        [Test]
        public void CalculateBookingDiscount_Given_EligibleCustomerAndProductDiscount_Then_CalculatesDiscount()
        {
            //Arrange
            var treatmentDiscount = 0.10m;
            var treatmentPrice = 200m;
            var productDiscount = 0.05m;
            var productPrice = 50m;
            var quantity = 2;

            var loyaltyDiscount = Fixture.New<LoyaltyDiscount>()
                .With(l => l.MinimumVisits, 5)
                .With(l => l.TreatmentDiscountPercent.Value, treatmentDiscount)
                .With(l => l.ProductDiscountPercent!.Value, productDiscount)
                .With(l => l.Name, "Test discount Name")
                .Build();

            var productLine = Fixture.New<ProductLine>()
                .With(pl => pl.Quantity.Value, quantity)
                .With(pl => pl.Product.Price.Value, productPrice)
                .Build();

            var booking = Fixture.New<Booking>()
                .With(b => b.Customer!.Visits, 6)
                .With(b => b.Treatment!.Price.Value, treatmentPrice)
                .With(b => b.ProductLines, [productLine])
                .Build();

            var expected = treatmentDiscount * treatmentPrice + quantity * productPrice * productDiscount;

            //Act
            var discount = loyaltyDiscount.CalculateBookingDiscount(booking);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(discount.Amount, Is.EqualTo(expected));
                Assert.That(discount.Type, Is.EqualTo(DiscountType.LoyaltyDiscount));
                Assert.That(discount.Name, Is.EqualTo(loyaltyDiscount.Name));
                Assert.That(discount.DiscountActive, Is.True);
            }
        }

        [Test]
        public void CalculateBookingDiscount_Given_IneligibleCustomer_Then_ReturnsInactiveDiscount()
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
