using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Tests
{
    // Mikkel Dahlmann

    internal sealed class AddressTests
    {
        [Test]
        public void Given_ValidAddressWithFloor_Then_CreatesAddress()
        {
            // Arrange
            var streetName = "Nygade";
            var city = "Vejle";
            var streetNumber = "10";
            var floor = 2;
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode, floor);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testAddress.StreetName, Is.EqualTo(streetName));
                Assert.That(testAddress.City, Is.EqualTo(city));
                Assert.That(testAddress.StreetNumber, Is.EqualTo(streetNumber));
                Assert.That(testAddress.Floor, Is.EqualTo(floor));
                Assert.That(testAddress.ZipCode, Is.EqualTo(zipCode));
            });
        }

        [Test]
        public void Given_ValidAddressWithoutFloor_Then_CreatesAddressWithNullFloorValue()
        {
            // Arrange
            var streetName = "Nygade";
            var city = "Vejle";
            var streetNumber = "10";
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testAddress.StreetName, Is.EqualTo(streetName));
                Assert.That(testAddress.City, Is.EqualTo(city));
                Assert.That(testAddress.StreetNumber, Is.EqualTo(streetNumber));
                Assert.That(testAddress.Floor, Is.Null);
                Assert.That(testAddress.ZipCode, Is.EqualTo(zipCode));
            });
        }

        [Test]
        public void Given_ValidAddressWithWhitespaceCity_Then_CreatesAddress()
        {
            // Arrange
            var streetName = "Nygade";
            var city = "Nørre Aaby";
            var streetNumber = "10";
            var floor = 2;
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode, floor);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testAddress.StreetName, Is.EqualTo(streetName));
                Assert.That(testAddress.City, Is.EqualTo(city));
                Assert.That(testAddress.StreetNumber, Is.EqualTo(streetNumber));
                Assert.That(testAddress.Floor, Is.EqualTo(floor));
                Assert.That(testAddress.ZipCode, Is.EqualTo(zipCode));
            });
        }

        [Test]
        public void Given_ValidAddressWithWhitespaceStreetName_Then_CreatesAddress()
        {
            // Arrange
            var streetName = "Ny Banegade";
            var city = "Vejle";
            var streetNumber = "10";
            var floor = 2;
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode, floor);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testAddress.StreetName, Is.EqualTo(streetName));
                Assert.That(testAddress.City, Is.EqualTo(city));
                Assert.That(testAddress.StreetNumber, Is.EqualTo(streetNumber));
                Assert.That(testAddress.Floor, Is.EqualTo(floor));
                Assert.That(testAddress.ZipCode, Is.EqualTo(zipCode));
            });
        }

        [Test]
        public void Given_ValidAddressWithFloor_Then_CreatesCorrectFullAddressString()
        {
            // Arrange
            var streetName = "Nygade";
            var city = "Vejle";
            var streetNumber = "10";
            var floor = 2;
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode, floor);

            // Assert
            Assert.That(testAddress.FullAddress, Is.EqualTo($"{streetName} {streetNumber}, {floor}. sal, {zipCode} {city}"));
        }

        [Test]
        public void Given_ValidAddressWithoutFloor_Then_CreatesCorrectFullAddressString()
        {
            // Arrange
            var streetName = "Nygade";
            var city = "Vejle";
            var streetNumber = "10";
            var zipCode = 7100;

            // Act
            var testAddress = Address.Create(streetName, city, streetNumber, zipCode);

            // Assert
            Assert.That(testAddress.FullAddress, Is.EqualTo($"{streetName} {streetNumber}, {zipCode} {city}"));
        }

        [TestCase("Nygade5", "Vejle", "10", 2, 7100)]
        [TestCase("Nygade", "Vejle5", "10", 2, 7100)]
        [TestCase("Nygade", "Vejle", "10-", 2, 7100)]
        [TestCase("Nygade", "Vejle", "10", -2, 7100)]
        [TestCase("Nygade", "Vejle", "10", 2, 71005)]
        public void Given_InvalidParameter_Then_ThrowsAddressException(string streetName, string city,
            string streetNumber, int floor, int zipCode)
        {
            // Arrange & Act & Assert
            Assert.Throws<AddressException>(() => Address.Create(streetName, city, streetNumber, zipCode, floor));
        }

    }
}
