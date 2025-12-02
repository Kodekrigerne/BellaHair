namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Value object representing a quantity of an item. Must be a positive number.
    /// </summary>
    public record Quantity
    {
        public int Value { get; private init; }

        private Quantity() { }

        private Quantity(int value)
        {
            Value = value;
        }

        public static Quantity FromInt(int quantity)
        {
            if (quantity < 1) throw new QuantityException("Mængde skal være 1 eller flere.");

            return new(quantity);
        }
    }

    public class QuantityException(string message) : DomainException(message);
}
