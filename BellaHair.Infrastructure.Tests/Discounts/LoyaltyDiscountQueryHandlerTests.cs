using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Ports.Discounts;

namespace BellaHair.Infrastructure.Tests.Discounts
{
    internal sealed class LoyaltyDiscountQueryHandlerTests : InfrastructureTestBase
    {
        [Test]
        public void GetAllAsync_GetsLoyaltyDiscounts()
        {
            //Arrange
            var handler = new LoyaltyDiscountQueryHandler(_db) as ILoyaltyDiscountQuery;

            var discount1 = LoyaltyDiscount.Create("First discount", 5, DiscountPercent.FromDecimal(0.05m));
            var discount2 = LoyaltyDiscount.Create("Second discount", 10, DiscountPercent.FromDecimal(0.10m));

            _db.AddRange(discount1, discount2);
            _db.SaveChanges();

            //Act
            var discounts = handler.GetAllAsync().GetAwaiter().GetResult();

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(discounts, Has.Count.EqualTo(2));
                Assert.That(discounts.Any(l => l.Id == discount1.Id), Is.True);
                Assert.That(discounts.Any(l => l.Id == discount2.Id), Is.True);
            }
        }

        [Test]
        public void GetCountAsync_GetsLoyaltyDiscountCount()
        {
            //Arrange
            var handler = new LoyaltyDiscountQueryHandler(_db) as ILoyaltyDiscountQuery;

            var discount1 = LoyaltyDiscount.Create("First discount", 5, DiscountPercent.FromDecimal(0.05m));
            var discount2 = LoyaltyDiscount.Create("Second discount", 10, DiscountPercent.FromDecimal(0.10m));

            _db.AddRange(discount1, discount2);
            _db.SaveChanges();

            //Act
            var count = handler.GetCountAsync().GetAwaiter().GetResult();

            //Assert
            Assert.That(count, Is.EqualTo(2));
        }
    }
}
