using BellaHair.Domain.Discounts;
using System.Globalization;

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
            var percent = decimal.Parse(percentStr, CultureInfo.InvariantCulture);

            var discountPercent = DiscountPercent.FromDecimal(percent);

            Assert.That(discountPercent.Value, Is.EqualTo(percent));
        }

        [TestCase("-0.1")]
        [TestCase("1.1")]
        public void Given_InvalidNumber_Then_ThrowsException(string percentStr)
        {
            var percent = decimal.Parse(percentStr, CultureInfo.InvariantCulture);

            Assert.Throws<DiscountPercentException>(() => DiscountPercent.FromDecimal(percent));
        }
    }
}
