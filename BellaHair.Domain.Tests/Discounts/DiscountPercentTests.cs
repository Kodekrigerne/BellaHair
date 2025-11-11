using System.Globalization;
using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Tests.Discounts
{
    //Dennis
    internal sealed class DiscountPercentTests
    {
        [TestCase("0")]
        [TestCase("0.5")]
        [TestCase("1.0")]
        public void Given_ValidNumber_Then_ReturnsDiscountPercent(string percentStr)
        {
            //Arrange
            var percent = decimal.Parse(percentStr, CultureInfo.InvariantCulture);

            //Act
            var discountPercent = DiscountPercent.FromDecimal(percent);

            //Assert
            Assert.That(discountPercent.Value, Is.EqualTo(percent));
        }

        [TestCase("-0.1")]
        [TestCase("1.1")]
        public void Given_InvalidNumber_Then_ThrowsException(string percentStr)
        {
            //Arrange
            var percent = decimal.Parse(percentStr, CultureInfo.InvariantCulture);

            //Act & Assert
            Assert.Throws<DiscountPercentException>(() => DiscountPercent.FromDecimal(percent));
        }
    }
}
