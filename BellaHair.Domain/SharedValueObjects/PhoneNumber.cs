using SharedKernel;

namespace BellaHair.Domain.SharedValueObjects
{
    /// <summary>
    /// Phone number value object with validation logic.
    /// </summary>

    // Linnea
    public record PhoneNumber
    {
        public string Value { get; private init; }

#pragma warning disable CS8618
        protected PhoneNumber() { }
#pragma warning restore CS8618

        private PhoneNumber(string value)
        {
            ValidateNumberLength(value);
            Value = value;
        }

        public static PhoneNumber FromString(string value) => new(value);

        /// <summary>
        /// Checks if number is 8 digits and throws exceptions if its under or below.
        /// Checks if number only contains digits and throws exception if it contains other characters
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="PhoneNumberException"></exception>
        private static void ValidateNumberLength(string value)
        {
            if (value.Length > 8) throw new PhoneNumberException("The number is too long.");
            if (value.Length < 8) throw new PhoneNumberException("The number is not long enough.");
            if (!value.All(Char.IsDigit)) throw new PhoneNumberException("The number has invalid characters.");
        }
    }

    public class PhoneNumberException(string message) : DomainException(message);
}
