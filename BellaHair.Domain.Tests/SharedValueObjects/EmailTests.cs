using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.SharedValueObjects
{
    internal sealed class EmailTests
    {
        [TestCase("a@gmail.com")]
        [TestCase("123@aaa.dk")]
        [TestCase("jørgen@123.net")]
        public void Given_ValidEmail_Then_CreatesEmail(string value)
        {
            //Act
            Email createdEmail = Email.FromString(value);

            //Assert
            Assert.That(createdEmail.Value, Is.EqualTo(value));
        }

        [TestCase("@gmail.com")]
        [TestCase("123@.dk")]
        [TestCase("jørgen@123.")]
        [TestCase("@@?.com")]
        public void Given_InvalidEmail_Then_ThrowsException(string value)
        {
            //Act & Assert
            Assert.Throws<EmailException>(() => Email.FromString(value));
        }
    }
}
