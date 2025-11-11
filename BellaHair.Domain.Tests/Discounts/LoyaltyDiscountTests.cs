using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Tests.Discounts
{
    internal sealed class LoyaltyDiscountTests
    {
        [TestCase("1")]
        [TestCase("5")]
        public void Create_Given_ValidParameters_Then_ConstructsLoyaltyDiscount(string visitsStr)
        {
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var discountPercent = DiscountPercent.FromDecimal(0.5m);

            var loyaltyDiscount = LoyaltyDiscount.Create(name, minimumVisits, discountPercent);

            Assert.Multiple(() =>
            {
                Assert.That(loyaltyDiscount.Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(loyaltyDiscount.Name, Is.EqualTo(name));
                Assert.That(loyaltyDiscount.MinimumVisits, Is.EqualTo(minimumVisits));
                Assert.That(loyaltyDiscount.DiscountPercent.Value, Is.EqualTo(discountPercent.Value));
            });
        }

        [TestCase("-5")]
        [TestCase("0")]
        public void Create_Given_InvalidMinimumVisits_Then_ThrowsException(string visitsStr)
        {
            var name = "Test discount name";
            var minimumVisits = int.Parse(visitsStr);
            var discountPercent = DiscountPercent.FromDecimal(0.5m);

            Assert.Throws<LoyaltyDiscountException>(() => LoyaltyDiscount.Create(name, minimumVisits, discountPercent));
        }
    }
}
