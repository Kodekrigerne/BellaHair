namespace BellaHair.Domain.Treatments.Value_Objects
{
    //Mikkel Klitgaard
    public record Duration
    {
        public int Value { get; private init; }


        protected Duration() { }


        private Duration(int value)
        {
            ValidateDuration(value);
            Value = value;
        }

        public static Duration FromInt(int duration) => new Duration(duration);


        public static void ValidateDuration(int value)
        {
            if (value < 10)
                throw new DurationException("The duration must be at least 10 minutes.");
            if (value > 300)
                throw new DurationException("The duration must be less than 5 hours.");
        }


    }

    public class DurationException(string message) : DomainException(message);
}
