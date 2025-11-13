using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Tests
{
    internal sealed class EmployeeTests
    {
        [TestCase]
        public void Given_ValidInput_Then_CreatesEmployee()
        {
            //Arrange
            Name name = Name.FromStrings("Lars", "Nielsen");
            Address adress = Address.Create("Nørregade", "Vejle", "2", 7100);
            PhoneNumber phoneNumber = PhoneNumber.FromString("12345678");
            Email email = Email.FromString("larsnielsen@mail.com");

            //Act
            Employee employee = Employee.Create(name, email, phoneNumber, adress);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(employee.Name.FullName, Is.EqualTo(name.FullName));
                Assert.That(employee.PhoneNumber.Value, Is.EqualTo(phoneNumber.Value));
                Assert.That(employee.Email.Value, Is.EqualTo(email.Value));
            });
        }

    }
}
