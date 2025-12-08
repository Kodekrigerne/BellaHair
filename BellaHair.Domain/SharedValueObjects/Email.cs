using SharedKernel;
using System.Text.RegularExpressions;

namespace BellaHair.Domain.SharedValueObjects
{

    /// <summary>
    /// Email value object that contains logic to validate email
    /// </summary>
    
    // Linnea
    public record Email
    {
        public string Value { get; private init; }
        // Et kompileret regulært udtryk, der bruges til at validere, at en del af navnet kun indeholder acceptable tegn.
        private static readonly Regex ValidEmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

#pragma warning disable CS8618
        protected Email() { }
#pragma warning restore CS8618

        private Email(string value)
        {
            ValidateEmail(value);
            Value = value;
        }

        public static Email FromString(string value) => new(value);

        public static void ValidateEmail(string value)
        {
            if (!ValidEmailRegex.IsMatch(value))
            {
                throw new EmailException($"The provided email address '{value}' is not in a valid format.");
            }
        }

    }

    public class EmailException(string message) : DomainException(message);
}
