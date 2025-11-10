using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BellaHair.Domain.Tests.SharedValueObjects
{
    public record Email
    {
        public string Value { get; private init; }

        // A compiled regular expression used to validate that an email format is correct.
        private static readonly Regex ValidEmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private Email(string value)
        {
            Value = value;
        }

        public static Email FromString(string value) => new(value);

        public void ValidateEmail(string value)
        {
            if (!ValidEmailRegex.IsMatch(value))
            {
                throw new EmailException($"The provided email address '{value}' is not in a valid format.");
            }
        }

    }

    public class EmailException(string message) : Exception(message);
}
