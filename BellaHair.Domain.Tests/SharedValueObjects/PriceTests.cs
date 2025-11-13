using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.SharedValueObjects
{
    // Mikkel Klitgaard
    internal sealed class PriceTests
    {
        [TestCase(0)]
        [TestCase(-5)]
        public void Given_PriceIsZeroOrLess_Then_ThrowsException(decimal value)
        {
            // Act & Assert
            Assert.Throws<PriceException>(() => Price.FromDecimal(value));
        }

        [TestCase(100_001)]
        [TestCase(200_000)]
        public void Given_PriceIsGreaterThan100K_Then_ThrowsException(decimal value)
        {
            // Act & Assert
            Assert.Throws<PriceException>(() => Price.FromDecimal(value));
        }

        [TestCase(1)]
        [TestCase(100_000)]
        [TestCase(1000)]
        public void Given_PriceIsValid_Then_ConstructsPrice(decimal value)
        {
            // Act
            Price validPrice = Price.FromDecimal(value);

            // Assert
            Assert.That(validPrice.Value, Is.EqualTo(value));
        }

    }
}
