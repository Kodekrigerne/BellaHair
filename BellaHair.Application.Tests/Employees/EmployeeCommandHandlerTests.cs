using BellaHair.Domain.Employees;
using BellaHair.Infrastructure.Employees;
using BellaHair.Ports.Employees;
using BellaHair.Application.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Treatments;
using BellaHair.Infrastructure.Treatments;
using Microsoft.Extensions.DependencyInjection;


namespace BellaHair.Application.Tests.Employees
{
    internal sealed class EmployeeCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public void Given_EmployeeData_Then_CreatesEmployeeInDatabase()
        {
            // Arrange
            IEnumerable<Guid> treatmentIds = [];

            var handler = ServiceProvider.GetRequiredService<IEmployeeCommand>();
            var command = new CreateEmployeeCommand("Martin", "Jensen", "test@mail.dk", "12345678", "Vejnavn", "Byen", "12", 1234, treatmentIds);

            // Act
            handler.CreateEmployeeCommand(command).GetAwaiter().GetResult();
            var employeeFromDB = _db.Employees.FirstOrDefault();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(employeeFromDB!.Name.FirstName, Is.EqualTo(command.FirstName));
                Assert.That(employeeFromDB!.Name.MiddleName, Is.EqualTo(command.MiddleName));
                Assert.That(employeeFromDB!.Name.LastName, Is.EqualTo(command.LastName));
                Assert.That(employeeFromDB!.Email.Value, Is.EqualTo(command.Email));
                Assert.That(employeeFromDB!.PhoneNumber.Value, Is.EqualTo(command.PhoneNumber));
                Assert.That(employeeFromDB!.Address.StreetName, Is.EqualTo(command.StreetName));
                Assert.That(employeeFromDB!.Address.City, Is.EqualTo(command.City));
                Assert.That(employeeFromDB!.Address.StreetNumber, Is.EqualTo(command.StreetNumber));
                Assert.That(employeeFromDB!.Address.ZipCode, Is.EqualTo(command.ZipCode));
                Assert.That(employeeFromDB!.Treatments.Count, Is.EqualTo(command.TreatmentIds.Count()));
            });
        }
    }

}

