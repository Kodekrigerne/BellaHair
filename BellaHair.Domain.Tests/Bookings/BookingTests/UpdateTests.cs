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

            booking.Update(startDateTime, employee, treatment, [], dateTimeProvider.Object);

            using (Assert.EnterMultipleScope())
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
                Assert.That(booking.TotalBase, Is.EqualTo(treatment.Price.Value));
            }
        }

        [Test]
        public void Given_BookingValidForUpdate_Then_UpdatesBookingWithProducts()
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

            var productLineData = Fixture.New<ProductLineData>()
                .With(pld => pld.Product.Id, Guid.NewGuid())
                .With(pld => pld.Product.Name, "Test product name")
                .With(pld => pld.Product.Description, "Test product description")
                .With(pld => pld.Product.Price.Value, 200m)
                .With(pld => pld.Quantity.Value, 3)
                .Build();

            booking.Update(startDateTime, employee, treatment, [productLineData], dateTimeProvider.Object);

            //Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(booking.ProductLines, Has.Count.EqualTo(1));
                Assert.That(booking.ProductLines[0].Product.Id, Is.EqualTo(productLineData.Product.Id));
                Assert.That(booking.ProductLines[0].Product.Name, Is.EqualTo(productLineData.Product.Name));
                Assert.That(booking.ProductLines[0].Product.Description, Is.EqualTo(productLineData.Product.Description));
                Assert.That(booking.ProductLines[0].Product.Price.Value, Is.EqualTo(productLineData.Product.Price.Value));
                Assert.That(booking.ProductLines[0].Quantity.Value, Is.EqualTo(productLineData.Quantity.Value));
            }
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

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, [], dateTimeProvider.Object));
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

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, [], dateTimeProvider.Object));
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

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, [], dateTimeProvider.Object));
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

            Assert.Throws<BookingException>(() => booking.Update(startDateTime, employee, treatment, [], dateTimeProvider.Object));
        }
    }
}
