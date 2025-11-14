using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BellaHair.Domain
{
    // Mikkel Dahlmann

    /// <summary>
    /// Represents a postal address, including street name, street number, city, zip code, and optional floor
    /// information. Provides validation of each property.
    /// </summary>
    
    // Nedarver ikke fra EntityBase. Bruger ikke ID. Defineres som en complextype gennem EF modelbuilder.
    public class Address
    {
        public string StreetName { get; private set; }
        public string City { get; private set; }
        public string StreetNumber { get; private set; }
        public int? Floor { get; private set; }
        public int ZipCode { get; private set; }
        public string FullAddress { get; private set; }

        // Regular Expression anvendt til trim af dobbelt whitespace.
        // Køres compile-time for at spare ressourcer ved runtime.
        private static readonly Regex WhiteSpaceRegex = new((@"\s+"), RegexOptions.Compiled);

        // Constructor til EF.
#pragma warning disable CS8618
        private Address() { }
#pragma warning restore CS8618


        // Offentlig metode til oprettelse af Address-objekt. Kalder privat constructor.
        public static Address Create(string streetName, string city, string streetNumber, int zipCode,
            int? floor = null) => new(streetName, city, streetNumber, zipCode, floor);


        // Privat constructor kalder validering af parametre, og sætter properties.
        private Address(string streetName, string city, string streetNumber, int zipCode, int? floor = null)
        {
            ValidateStreetAndCity(streetName);
            ValidateStreetAndCity(city);
            ValidateStreetNumber(streetNumber);
            ValidateZipCode(zipCode);
            if (floor != null) ValidateFloor(floor);

            StreetName = streetName.Trim();
            StreetName = WhiteSpaceRegex.Replace(StreetName, " ");

            City = city.Trim();
            City = WhiteSpaceRegex.Replace(City, " ");

            StreetNumber = streetNumber;
            ZipCode = zipCode;
            Floor = floor;

            FullAddress = BuildFullAddressString();
        }

        // Returnerer én samlet adresse-string sammensat af properties vha. StringBuilder.
        // Udelader floor såfremt denne er null.
        private string BuildFullAddressString()
        {
            var sb = new StringBuilder();

            sb.Append($"{StreetName} {StreetNumber}");

            if (!string.IsNullOrEmpty(Floor.ToString()))
            {
                sb.Append($", {Floor}. sal");
            }

            sb.Append($", {ZipCode} {City}");

            return sb.ToString();
        }


        private static void ValidateStreetAndCity(string name)
        {
            if (name.Any(x => !char.IsLetter(x) && x != '-' && x != ' '))
                throw new AddressException("Name can only consist of letters.");

            if (name.Length > 50)
                throw new AddressException("Name can not be longer than 30 characters.");
        }


        private static void ValidateStreetNumber(string streetNumber)
        {
            if (streetNumber.Any(x => !char.IsLetterOrDigit(x)))
                throw new AddressException("Streetnumber can only consist of numbers and letters.");
        }


        private static void ValidateZipCode(int zipCode)
        {
            if (zipCode > 9999 || zipCode < 1000)
                throw new AddressException("Zipcode is invalid.");
        }

        private static void ValidateFloor(int? floor)
        {
            if (floor < 1 || floor > 100)
                throw new AddressException("Floor is invalid, must be between 1 and 100.");
        }

        public void UpdateAddress(string streetName, string city, string streetNumber, int zipCode, int? floor)
        {
            ValidateStreetAndCity(streetName);
            ValidateStreetAndCity(city);
            ValidateStreetNumber(streetNumber);
            ValidateZipCode(zipCode);
            if (floor != null) ValidateFloor(floor);

            StreetName = streetName.Trim();
            StreetName = WhiteSpaceRegex.Replace(StreetName, " ");

            City = city.Trim();
            City = WhiteSpaceRegex.Replace(City, " ");

            StreetNumber = streetNumber;
            ZipCode = zipCode;
            Floor = floor;

            FullAddress = BuildFullAddressString();
        }
    }
    public class AddressException(string message) : DomainException(message);
}