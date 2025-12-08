using SharedKernel;

namespace BellaHair.Domain.SharedValueObjects
{
    // Mikkel Klitgaard

    /// <summary>
    /// Represents a price value within a valid range (between 1 - 100.000).
    /// Instances of this type are immutable and can only be created using the <see cref="FromDecimal(decimal)"/> method,
    /// which validates the input.
    /// </summary>
    public record Price
    {
        public decimal Value { get; private init; }

        protected Price() { }

        private Price(decimal value)
        {
            ValidatePrice(value);
            Value = value;
        }

        public static Price FromDecimal(decimal price) => new(price);

        public static void ValidatePrice(decimal value)
        {
            if (value < 1m)
                throw new PriceException("Price cannot be zero or less");
            if (value > 100_000m)
                throw new PriceException("Price cannot exceed 100.000");
        }
    }
    public class PriceException(string message) : DomainException(message);
}
