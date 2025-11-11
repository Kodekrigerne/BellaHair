using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BellaHair.Domain.SharedValueObjects
{
    public partial record Address
    {
        public string StreetName { get; private init; }
        public string City { get; private init; }
        public string StreetNumber { get; private init; }
        public int? Floor { get; private init; }
        public int ZipCode { get; private init; }
        public string FullAddress { get; private init; }


        #pragma warning disable CS8618
        private Address() { }
        #pragma warning restore CS8618


        private Address(string streetName, string city, string streetNumber, int zipCode, int? floor = null)
        {
            ValidateStreetAndCity(streetName);
            ValidateStreetAndCity(city);
            ValidateStreetNumber(streetNumber);
            ValidateZipCode(zipCode);
            if (floor != null) ValidateFloor(floor);

            StreetName = streetName.Trim();
            StreetName = WhiteSpaceRegex().Replace(StreetName, " ");

            City = city.Trim();
            City = WhiteSpaceRegex().Replace(City, " ");
            
            StreetNumber = streetNumber;
            ZipCode = zipCode;
            Floor = floor;

            FullAddress = BuildFullAddressString(StreetName, City, StreetNumber, ZipCode, Floor);
        }


        private string BuildFullAddressString(string streetName, string city, string streetNumber, int zipCode, int? floor)
        {
            var sb = new StringBuilder();

            sb.Append($"{streetName} {streetNumber}");

            if (!string.IsNullOrEmpty(floor.ToString()))
            {
                sb.Append($", {floor}. sal");
            }

            sb.Append($", {zipCode} {city}");

            return sb.ToString();
        }


        private void ValidateStreetAndCity(string name)
        {
            if (!name.Any(x => char.IsLetter(x) || x == ' ')) 
                throw new AddressException("Name can only consist of letters.");
            
            if (name.Length > 50) 
                throw new AddressException("Name can not be longer than 30 characters.");
        }


        private void ValidateStreetNumber(string streetNumber)
        {
            if (!streetNumber.Any(char.IsLetterOrDigit))
                throw new AddressException("Streetnumber can only consist of numbers and letters.");
        }


        private void ValidateZipCode(int zipCode)
        {
            if (zipCode > 9999 || zipCode < 1000)
                throw new AddressException("Zipcode is invalid.");
        }

        private void ValidateFloor(int? floor)
        {
            if (floor < 1 || floor > 100)
                throw new AddressException("Floor is invalid, must be between 1 and 100.");
        }


        public static Address FromInputs(string streetName, string city, string streetNumber, int zipCode,
            int? floor = null) => new(streetName, city, streetNumber, zipCode, floor);


        [GeneratedRegex(@"\s+")]
        private static partial Regex WhiteSpaceRegex();
    }


    public class AddressException(string message) : Exception(message);
}
