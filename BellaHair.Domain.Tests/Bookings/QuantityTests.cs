using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Tests.Bookings
{
    public sealed class QuantityTests
    {
        [TestCase(1)]
        [TestCase(5)]
        public void Create_Given_ValidInteger_Then_CreatesQuantity(int input)
        {
            //Act
            var quantity = Quantity.FromInt(input);

            //Assert
            Assert.That(quantity.Value, Is.EqualTo(input));
        }

        [TestCase(0)]
        [TestCase(-5)]
        public void Create_Given_InvalidInteger_Then_ThrowsException(int input)
        {
            Assert.Throws<QuantityException>(() => Quantity.FromInt(input));
        }
    }
}
