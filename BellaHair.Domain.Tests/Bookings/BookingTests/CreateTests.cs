using BellaHair.Domain.Bookings;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using FixtureBuilder;
using Moq;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class CreateTests
    {
        [Test]
        public void Given_ValidParameters_Then_CreatesBooking()
        {
            //Arrange
            var customer = Fixture.New<PrivateCustomer>().With(p => p.Id, Guid.NewGuid()).Build();
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var startDateTime = DateTime.Now.AddMinutes(5);
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            //Act
            var booking = Booking.Create(customer, employee, treatment, startDateTime, dateTimeProvider.Object);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(booking.Employee, Is.Not.Null);
                Assert.That(booking.Employee!.Id, Is.EqualTo(employee.Id));
                Assert.That(booking.Treatment, Is.Not.Null);
                Assert.That(booking.Treatment!.Id, Is.EqualTo(treatment.Id));
                Assert.That(booking.Customer, Is.Not.Null);
                Assert.That(booking.Customer!.Id, Is.EqualTo(customer.Id));
                Assert.That(booking.IsPaid, Is.False);
                Assert.That(booking.StartDateTime, Is.EqualTo(startDateTime));
                Assert.That(booking.PaidDateTime, Is.Null);
                Assert.That(booking.CustomerSnapshot, Is.Null);
                Assert.That(booking.TreatmentSnapshot, Is.Null);
                Assert.That(booking.EmployeeSnapshot, Is.Null);
            });
        }

        [Test]
        public void Given_EmployeeNotHasTreatment_Then_ThrowsException()
        {
            //Arrange
            var customer = Fixture.New<PrivateCustomer>().With(p => p.Id, Guid.NewGuid()).Build();
            var treatment1 = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var treatment2 = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment1]).Build();
            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            //Act & Assert
            Assert.Throws<BookingException>(
                () => Booking.Create(customer, employee, treatment2, DateTime.Now.AddMinutes(5), dateTimeProvider.Object));
        }
    }
}
