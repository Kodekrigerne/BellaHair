namespace BellaHair.Domain.Treatments.Value_Objects
{
    public record Price
    {
        public decimal Value { get; private init; }


        protected Price() { }


        private Price(decimal value)
        {
            Value = value;
        }

        public static Price FromDecimal(decimal price) => new Price(price);

        public static void ValidatePrice(decimal value)
        {
            if (value < 0)
                throw new PriceException("Price cannot be less than zero.");
            if (value > 100000)
                throw new PriceException("Price cannot exceed 100.000");
        }

    }

    public class PriceException(string message) : Exception(message);
}
