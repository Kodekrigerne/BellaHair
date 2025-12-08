using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Tests.Discounts
{
    // Mikkel Klitgaard

    internal sealed class BirthdayDiscountTests
    {
        [Test]
        public void Given_ValidBirthdayDiscount_Then_CreatesBirthdayDiscount()
        {
            // Arrange 

            var name = "Fødselsdagsrabat";
            var discountPercent = DiscountPercent.FromDecimal(0.50m);

            // Act

            var birthdayDiscount = BirthdayDiscount.Create(name, discountPercent);

            // Assert

            using (Assert.EnterMultipleScope())
            {
                Assert.That(birthdayDiscount.Name, Is.EqualTo(name));
                Assert.That(birthdayDiscount.DiscountPercent, Is.EqualTo(discountPercent));
            }

        }

    }
}
