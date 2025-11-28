using BellaHair.Domain.Bookings;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Treatments;
using FixtureBuilder;
using Moq;

namespace BellaHair.Domain.Tests.Bookings.BookingTests
{
    internal sealed class UpdateTests
    {
        [Test]
        public void Given_BookingValidForUpdate_Then_UpdatesBooking()
        {
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var startDateTime = DateTime.Now.AddMinutes(5);

            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            booking.Update(startDateTime, employee, treatment, dateTimeProvider.Object);

            Assert.Multiple(() =>
            {
                Assert.That(booking.Employee, Is.Not.Null);
                Assert.That(booking.Employee!.Id, Is.EqualTo(employee.Id));
                Assert.That(booking.Treatment, Is.Not.Null);
                Assert.That(booking.Treatment!.Id, Is.EqualTo(treatment.Id));
                Assert.That(booking.IsPaid, Is.False);
                Assert.That(booking.StartDateTime, Is.EqualTo(startDateTime));
                Assert.That(booking.PaidDateTime, Is.Null);
                Assert.That(booking.CustomerSnapshot, Is.Null);
                Assert.That(booking.TreatmentSnapshot, Is.Null);
                Assert.That(booking.EmployeeSnapshot, Is.Null);
                Assert.That(booking.EndDateTime, Is.EqualTo(startDateTime.AddMinutes(treatment.DurationMinutes.Value)));
                Assert.That(booking.Total, Is.EqualTo(treatment.Price.Value));
            });
        }

        [Test]
        public void Given_EmployeeNotHasTreatment_Then_ThrowsException()
        {
            var treatment = Fixture.New<Treatment>().Build();
            var employee = Fixture.New<Employee>().Build();
            var startDateTime = DateTime.Now.AddMinutes(5);

            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, dateTimeProvider.Object));
        }

        [Test]
        public void Given_UnpaidPastBooking_Then_ThrowsException()
        {
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var startDateTime = DateTime.Now.AddMinutes(5);

            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(-5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, dateTimeProvider.Object));
        }

        [Test]
        public void Given_UnpaidFutureBooking_Then_ThrowsException()
        {
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var startDateTime = DateTime.Now.AddMinutes(5);

            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, true)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, dateTimeProvider.Object));
        }

        [Test]
        public void Given_StartBeforeNow_Then_ThrowsException()
        {
            var treatment = Fixture.New<Treatment>().With(t => t.Id, Guid.NewGuid()).Build();
            var employee = Fixture.New<Employee>().With(e => e.Id, Guid.NewGuid()).WithField("_treatments", [treatment]).Build();
            var startDateTime = DateTime.Now.AddMinutes(-5);

            var booking = Fixture.New<Booking>()
                .With(b => b.IsPaid, false)
                .With(b => b.StartDateTime, DateTime.Now.AddHours(5))
                .Build();

            var dateTimeProvider = new Mock<ICurrentDateTimeProvider>();
            dateTimeProvider.Setup(d => d.GetCurrentDateTime()).Returns(DateTime.Now);

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, dateTimeProvider.Object));
        }
    }
}
