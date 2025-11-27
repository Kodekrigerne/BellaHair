using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Employees
{
    internal sealed class EmployeeTests
    {
        [TestCase]
        public void Given_ValidInputs_Then_CreatesEmployee()
        {

            // Arrange
            Name name = Name.FromStrings("Lars", "Nielsen");
            Address address = Address.Create("Nørregade", "Vejle", "2", 7100);
            PhoneNumber phoneNumber = PhoneNumber.FromString("12345678");
            Email email = Email.FromString("larsnielsen@mail.com");
            
            var treatment1 = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var treatment2 = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var treatment3 = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();

            List<Treatment> treatments = [treatment1, treatment2, treatment3];

            // Act
            Employee employee = Employee.Create(name, email, phoneNumber, address, treatments);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(employee.Name.FullName, Is.EqualTo(name.FullName));
                Assert.That(employee.PhoneNumber.Value, Is.EqualTo(phoneNumber.Value));
                Assert.That(employee.Email.Value, Is.EqualTo(email.Value));
                Assert.Multiple(() =>
                {
                    Assert.That(employee.Treatments, Does.Contain(treatment1));
                    Assert.That(employee.Treatments, Does.Contain(treatment2));
                    Assert.That(employee.Treatments, Does.Contain(treatment1));
                });
            });
        }

        [TestCase]
        public void Given_ValidInputs_Then_UpdatesEmployee()
        {
            // Arrange
            Name name = Name.FromStrings("Lars", "Nielsen");
            Address address = Address.Create("Nørregade", "Vejle", "2", 7100);
            PhoneNumber phoneNumber = PhoneNumber.FromString("12345678");
            Email email = Email.FromString("larsnielsen@mail.com");

            List<Treatment> treatments = [Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build() ];

            Employee employee = Employee.Create(name, email, phoneNumber, address, treatments);

            PhoneNumber newPhoneNumber = PhoneNumber.FromString("87654321");

            // Act
            employee.Update(name, email, newPhoneNumber, address, treatments);

            // Assert
            Assert.That(employee.PhoneNumber, Is.EqualTo(newPhoneNumber));
        }

    }
}
