using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.SharedValueObjects
{
    /// <summary>
    /// Phone number value object with validation logic.
    /// </summary>
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
        /// <exception cref="NumberException"></exception>
        private void ValidateNumberLength(string value)
        {
            if (value.Length > 8) throw new NumberException("The number is too long.");
            if (value.Length < 8) throw new NumberException("The number is not long enough.");
            if (!value.All(Char.IsDigit)) throw new NumberException("The number has invalid characters."); 
        }
    }

    public class NumberException(string message) : Exception(message);
}
