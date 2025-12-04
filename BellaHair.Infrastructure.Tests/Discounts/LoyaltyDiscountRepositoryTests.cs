using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure.Discounts;

namespace BellaHair.Infrastructure.Tests.Discounts
{
    internal sealed class LoyaltyDiscountRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void Add_AddsLoyaltyDiscount()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db) as ILoyaltyDiscountRepository;
            var discount = LoyaltyDiscount.Create("Test name", 5, DiscountPercent.FromDecimal(0.05m));

            //Act
            repo.AddAsync(discount);
            _db.SaveChanges();

            //Assert
            var discountFromDb = _db.Discounts.Single() as LoyaltyDiscount;

            Assert.Multiple(() =>
            {
                Assert.That(discountFromDb!.Id, Is.EqualTo(discount.Id));
                Assert.That(discountFromDb!.Name, Is.EqualTo(discount.Name));
                Assert.That(discountFromDb!.MinimumVisits, Is.EqualTo(discount.MinimumVisits));
                Assert.That(discountFromDb!.TreatmentDiscountPercent.Value, Is.EqualTo(discount.TreatmentDiscountPercent.Value));
            });
        }

        [Test]
        public void Delete_DeletesLoyaltyDiscount()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db) as ILoyaltyDiscountRepository;
            var discount = LoyaltyDiscount.Create("Test name", 5, DiscountPercent.FromDecimal(0.05m));

            _db.Add(discount);
            _db.SaveChanges();

            var discountFromDb = _db.Discounts.Single() as LoyaltyDiscount;

            //Act
            repo.Delete(discountFromDb!);
            _db.SaveChanges();

            //Assert
            Assert.That(_db.Discounts.Any(), Is.False);
        }

        [Test]
        public void Get_Given_ValidData_Then_GetsLoyaltyDiscount()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db) as ILoyaltyDiscountRepository;
            var discount = LoyaltyDiscount.Create("Test name", 5, DiscountPercent.FromDecimal(0.05m));

            _db.Add(discount);
            _db.SaveChanges();

            //Act
            var discountFromRepo = repo.GetAsync(discount.Id).GetAwaiter().GetResult();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(discountFromRepo.Id, Is.EqualTo(discount.Id));
                Assert.That(discountFromRepo.Name, Is.EqualTo(discount.Name));
                Assert.That(discountFromRepo.MinimumVisits, Is.EqualTo(discount.MinimumVisits));
                Assert.That(discountFromRepo.TreatmentDiscountPercent.Value, Is.EqualTo(discount.TreatmentDiscountPercent.Value));
            });
        }

        [Test]
        public void Get_Given_InvalidId_Then_ThrowsException()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db) as ILoyaltyDiscountRepository;
            var discount = LoyaltyDiscount.Create("Test name", 5, DiscountPercent.FromDecimal(0.05m));

            _db.Add(discount);
            _db.SaveChanges();

            //Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(Guid.NewGuid()));
        }

        //TODO: Implement when more discount types exist.
        //[Test]
        //public void Get_Given_WrongDiscountType_Then_ThrowsException()
        //{

        //}
    }
}
