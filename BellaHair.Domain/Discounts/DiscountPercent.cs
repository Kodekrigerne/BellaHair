
using SharedKernel;

namespace BellaHair.Domain.Discounts
{
    //Dennis
    /// <summary>
    /// Value object representing a percentage discount to be applied to a booking.
    /// </summary>
    /// <remarks>
    /// Given decimal must be between 0 and 1.
    /// </remarks>
    /// <exception cref="DiscountPercentException" />
    public record DiscountPercent
    {
        public decimal Value { get; private init; }

        private DiscountPercent(decimal value)
        {
            if (value < 0 || value > 1)
                throw new DiscountPercentException("Discount percent must not be outside of 0-1.");

            Value = value;
        }

        public static DiscountPercent FromDecimal(decimal discountPercent) => new(discountPercent);
    }

    public class DiscountPercentException(string message) : DomainException(message);
}
