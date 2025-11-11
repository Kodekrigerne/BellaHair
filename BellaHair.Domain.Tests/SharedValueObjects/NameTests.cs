using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.SharedValueObjects
{
    internal sealed class NameTests
    {
        [TestCase("Bo", "Jensen1", "Børge")]
        [TestCase("Bo1", "Jensen Hansen", "Børge")]
        [TestCase("Bo", "Jensen", "Børge1")]
        public void Given_FirstLastOrMiddleNameContainsNumber_Then_ThrowsException(string firstName, string lastName, string middleName)
        {
            //Arrange, Act & Assert
            Assert.Throws<NameException>(() => Name.FromStrings(firstName, lastName, middleName));
        }

        [TestCase("Bo@", "Jensen", "Børge")]
        [TestCase("Bo", "Jensen@", "Børge")]
        [TestCase("Bo", "Jensen", "Børge@")]
        public void Given_FirstLastOrMiddleNameContainsInvalidCharacter_Then_ThrowsException(string firstName, string lastName, string middleName)
        {
            //Arrange, Act & Assert
            Assert.Throws<NameException>(() => Name.FromStrings(firstName, lastName, middleName));
        }

        [Test]
        public void Given_MiddleNameIsNull_Then_CreatesName()
        {
            //Arrange
            string firstName = "Bo";
            string lastName = "Jensen";

            //Act   
            Name nameWithoutMiddleName = Name.FromStrings(firstName, lastName);

            //Assert
            Assert.That(nameWithoutMiddleName.MiddleName, Is.EqualTo(null));
        }

        [TestCase("Bo", "Jensen", "Børge")]
        [TestCase("Bo", "Jensen Hansen", "Børge")]
        [TestCase("Lars", "Nielsen", "Borup Hansen")]
        [TestCase("Mads Hansen", "Hansen", "Henny")]
        [TestCase("Mads Hansen", "Hansen Jensen", "Henny Benny")]
        [TestCase("Mads-Hansen", "Jensen", "Henny B.")]
        public void Given_FirstLastAndMiddleNameOnlyContainsValidCharacters_Then_CreatesName(string firstName, string lastName, string middleName)
        {
            //Arrange
            Name createdName = Name.FromStrings(firstName, lastName, middleName);

            //Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(createdName.FirstName, Is.EqualTo(firstName));
                Assert.That(createdName.LastName, Is.EqualTo(lastName));
                Assert.That(createdName.MiddleName, Is.EqualTo(middleName));
            });
        }
    }
}
