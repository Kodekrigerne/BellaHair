using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.SharedValueObjects
{
    internal sealed class PhoneNumberTests
    {
        [TestCase("32939590")]
        [TestCase("93183859")]
        [TestCase("47582893")]
        public void Given_ValidNumber_Then_CreatesPhoneNumber(string value)
        {
            //Act
            PhoneNumber createdNumber = PhoneNumber.FromString(value);

            //Assert
            Assert.That(createdNumber.Value, Is.EqualTo(value));
        }

        [TestCase("2777779")]
        [TestCase("1")]
        public void Given_NumberTooShort_Then_ThrowsException(string value)
        {
            //Act & Assert
            Assert.Throws<PhoneNumberException>(() => PhoneNumber.FromString(value));
        }

        [TestCase("123456789")]
        [TestCase("12345678910")]
        public void Given_NumberTooLong_Then_ThrowsException(string value)
        {
            //Act & Assert
            Assert.Throws<PhoneNumberException>(() => PhoneNumber.FromString(value));
        }

        [TestCase("abc12345")]
        [TestCase("@£123456")]
        [TestCase("-.,.12345")]
        public void Given_NumberContainsNonDigits_Then_ThrowsException(string value)
        {
            //Act & Assert
            Assert.Throws<PhoneNumberException>(() => PhoneNumber.FromString(value));
        }
    }
}
