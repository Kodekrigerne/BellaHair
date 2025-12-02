using BellaHair.Application.Discounts;
using BellaHair.Domain.Discounts;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Ports.Discounts;

namespace BellaHair.Application.Tests.Discounts
{
    internal sealed class LoyaltyDiscountCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public void CreateLoyaltyDiscountAsync_CreatesLoyaltyDiscount()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db);
            var handler = new LoyaltyDiscountCommandHandler(repo) as ILoyaltyDiscountCommand;
            var command = new CreateLoyaltyDiscountCommand("Test name", 5, 0.05m);

            //Act
            handler.CreateLoyaltyDiscountAsync(command);

            //Assert
            var discountFromDb = _db.Discounts.Single() as LoyaltyDiscount;

            Assert.Multiple(() =>
            {
                Assert.That(discountFromDb!.Name, Is.EqualTo(command.Name));
                Assert.That(discountFromDb!.MinimumVisits, Is.EqualTo(command.MinimumVisits));
                Assert.That(discountFromDb!.TreatmentDiscountPercent.Value, Is.EqualTo(command.DiscountPercent));
            });
        }

        [Test]
        public void DeleteLoyaltyDiscountAsync_DeletesLoyaltyDiscount()
        {
            //Arrange
            var repo = new LoyaltyDiscountRepository(_db);
            var handler = new LoyaltyDiscountCommandHandler(repo) as ILoyaltyDiscountCommand;
            var discount = LoyaltyDiscount.Create("Test name", 5, DiscountPercent.FromDecimal(0.05m));

            _db.Add(discount);
            _db.SaveChanges();

            var command = new DeleteLoyaltyDiscountCommand(discount.Id);

            //Act
            handler.DeleteLoyaltyDiscountAsync(command);

            //Assert
            Assert.That(_db.Discounts.Any(), Is.False);
        }
    }
}
