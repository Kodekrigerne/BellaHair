using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BellaHair.Domain.SharedValueObjects
{

    /// <summary>
    /// Name value object that contains firstname, lastname and optional (nullable) middle name.
    /// Has validation logic - currently only letters and one last name are allowed.
    /// </summary>
    
    public record Name
    {
        public string FirstName { get; private init; }
        public string LastName { get; private init; }
        public string? MiddleName { get; private init; }
        public string FullName { get; private init; }

        // A compiled regular expression used to validate that a name part contains only acceptable characters.
        private static readonly Regex ValidNamePartRegex = new Regex(@"^[\p{L}\s.'-]+$", RegexOptions.Compiled);

#pragma warning disable CS8618
        private Name() { }
        #pragma warning restore CS8618

        private Name(string firstName, string lastName, string? middleName = null)
        {
            if (!VerifyName(firstName)) throw new NameException("First name is invalid.");
            if (!VerifyName(lastName)) throw new NameException("Last name is invalid.");
            if (middleName != null)
            {
                if (!VerifyName(middleName)) throw new NameException("Middle name is invalid.");
            }

            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;

            if (middleName == null) FullName = $"{FirstName} {LastName}";       
            else FullName = $"{FirstName} {MiddleName} {LastName}";
        }

        public static Name FromStrings(string firstName, string lastName, string? middleName = null) => new(firstName, lastName, middleName);

        private static bool VerifyName(string name)
        {

            if (!ValidNamePartRegex.IsMatch(name)) return false;
            return true;
        }
    }

    public class NameException(string message): Exception(message);
}
