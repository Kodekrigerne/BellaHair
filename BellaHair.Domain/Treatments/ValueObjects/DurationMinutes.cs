namespace BellaHair.Domain.Treatments.ValueObjects
{
    //Mikkel Klitgaard

    /// <summary>
    /// Represents a durationMinutes value withing a valid range (between 10 minutes and 5 hours).
    /// Instances of this type are immutable and can only be created using the <see cref="FromInt(int)"/> method,
    /// which validates the input.
    /// </summary>
    public record DurationMinutes
    {
        public int Value { get; private init; }

        protected DurationMinutes() { }

        private DurationMinutes(int value)
        {
            ValidateDuration(value);
            Value = value;
        }

        public static DurationMinutes FromInt(int duration) => new DurationMinutes(duration);

        public static void ValidateDuration(int value)
        {
            if (value < 10)
                throw new DurationException("The durationMinutes must be at least 10 minutes.");
            if (value > 300)
                throw new DurationException("The durationMinutes must be less than 5 hours.");
        }
    }
    public class DurationException(string message) : DomainException(message);
}
