using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests.SharedValueObjects
{

    // TODO: Create more Email tests
    internal sealed class EmailTests
    {
        [TestCase("a@gmail.com")]
        [TestCase("123@aaa.dk")]
        [TestCase("jørgen@123.net")]
        public void Given_ValidNumber_Then_CreatesEmail(string value)
        {
            //Act
            Email createdEmail = Email.FromString(value);

            //Assert
            Assert.That(createdEmail.Value, Is.EqualTo(value));
        }
    }
}
