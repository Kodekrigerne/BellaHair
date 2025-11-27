using BellaHair.Domain;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Infrastructure.Employees;
using BellaHair.Ports.Employees;

namespace BellaHair.Infrastructure.Tests.Employees
{
    internal sealed class EmployeeQueryHandlerTests : InfrastructureTestBase
    {
        [Test]
        public void GetAllEmployeesSimpleAsync_GetsAllEmployees()
        {
            // Arrange

            var handler = (IEmployeeQuery)new EmployeeQueryHandler(_db, new CurrentDateTimeProvider());

            Name name1 = Name.FromStrings("Lars", "Nielsen");
            Address adress1 = Address.Create("Nørregade", "Vejle", "2", 7100);
            PhoneNumber phoneNumber1 = PhoneNumber.FromString("12345678");
            Email email1 = Email.FromString("larsnielsen@mail.com");
            List<Treatment> treatments1 = [];
            Employee employee1 = Employee.Create(name1, email1, phoneNumber1, adress1, treatments1);

            Name name2 = Name.FromStrings("Benny", "Jamz");
            Address adress2 = Address.Create("Vejlevej", "Fredericia", "4", 7000);
            PhoneNumber phoneNumber2 = PhoneNumber.FromString("23456789");
            Email email2 = Email.FromString("bennyz@mail.com");
            List<Treatment> treatments2 = [];
            Employee employee2 = Employee.Create(name2, email2, phoneNumber2, adress2, treatments2);

            _db.Add(employee1);
            _db.Add(employee2);

            _db.SaveChanges();

            // Act
            var employeesFromDb = handler.GetAllEmployeesAsync().GetAwaiter().GetResult();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(employeesFromDb, Has.Count.EqualTo(2));
                Assert.That(employeesFromDb.Any(t => t.Id == employee1.Id), Is.True);
                Assert.That(employeesFromDb.Any(t => t.Id == employee2.Id), Is.True);
            });

        }
    }
}
