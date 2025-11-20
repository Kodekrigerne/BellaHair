using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Tests.Employees
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
            Treatment treatment1 = Treatment.Create("Klipning herre", Price.FromDecimal(200), DurationMinutes.FromInt(30));
            Treatment treatment2 = Treatment.Create("Balyage", Price.FromDecimal(1000), DurationMinutes.FromInt(160));
            Treatment treatment3 = Treatment.Create("Klipning dame", Price.FromDecimal(250), DurationMinutes.FromInt(45));
            List<Treatment> treatments = [treatment1, treatment2, treatment3];

            //Act
            Employee employee = Employee.Create(name, email, phoneNumber, adress, treatments);

            //Assert
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

    }
}
