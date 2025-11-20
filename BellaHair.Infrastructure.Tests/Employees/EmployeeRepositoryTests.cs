using BellaHair.Domain;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Infrastructure.Discounts;
using BellaHair.Infrastructure.Employees;

namespace BellaHair.Infrastructure.Tests.Employees
{
    internal sealed class EmployeeRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void Given_NewEmployee_Then_AddsEmployeeToDatabase()
        {
            // Arrange
            var repo = (IEmployeeRepository)new EmployeeRepository(_db);

            var name = Name.FromStrings("Jens", "Jensen", "Jensen");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            List<Treatment> treatments = [];


            var employee = Employee.Create(name, email, phoneNumber, address, treatments);

            // Act
            repo.AddAsync(employee);

            // Assert
            _db.SaveChangesAsync();
            var actualEmployee = _db.Employees.First();
            Assert.That(actualEmployee.Id, Is.EqualTo(employee.Id));
        }

        [Test]
        public void Get_Given_ValidData_Then_GetsEmployee()
        {
            //Arrange
            var repo = (IEmployeeRepository)new EmployeeRepository(_db);

            var name = Name.FromStrings("Jens", "Jensen", "Jensen");
            var address = Address.Create("Vej", "By", "1", 7100, 2);
            var phoneNumber = PhoneNumber.FromString("12345678");
            var email = Email.FromString("email@email.com");
            List<Treatment> treatments = [];

            var employee = Employee.Create(name, email, phoneNumber, address, treatments);

            _db.Add(employee);
            _db.SaveChanges();

            //Act
            var employeeFromDb = repo.GetAsync(employee.Id).GetAwaiter().GetResult();

            //Assert
            Assert.That(employeeFromDb.Id, Is.EqualTo(employee.Id));
        }

    }
}
