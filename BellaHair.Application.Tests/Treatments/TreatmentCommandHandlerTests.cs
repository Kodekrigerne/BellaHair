using BellaHair.Application.Treatments;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure.PrivateCustomers;
using BellaHair.Infrastructure;
using BellaHair.Infrastructure.Bookings;
using BellaHair.Infrastructure.Treatments;
using BellaHair.Ports.Treatments;

namespace BellaHair.Application.Tests.Treatments
{
    // Mikkel Klitgaard

    internal sealed class TreatmentCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public void Given_Treatment_Then_CreatesTreatment()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var bookingChecker = (IFutureBookingWithTreatmentChecker)new FutureBookingWithTreatmentChecker(_db, dateTimeProvider);
            var handler = (ITreatmentCommand)new TreatmentCommandHandler(repo, bookingChecker);
            var command = new CreateTreatmentCommand("Herreklip", 450m, 45);

            // Act

            handler.CreateTreatmentAsync(command).GetAwaiter().GetResult();
            var treatmentFromDb = _db.Treatments.FirstOrDefault();

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(treatmentFromDb!.Name, Is.EqualTo(command.Name));
                Assert.That(treatmentFromDb!.Price.Value, Is.EqualTo(command.Price));
                Assert.That(treatmentFromDb!.DurationMinutes.Value, Is.EqualTo(command.DurationMinutes));
            });
        }

        [Test]
        public void Given_Treatment_Then_DeletesTreatment()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);
            var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            var bookingChecker = (IFutureBookingWithTreatmentChecker)new FutureBookingWithTreatmentChecker(_db, dateTimeProvider);
            var handler = (ITreatmentCommand)new TreatmentCommandHandler(repo, bookingChecker);
            var treatment = Treatment.Create("Herreklip", Price.FromDecimal(450), DurationMinutes.FromInt(45));

            _db.Add(treatment);
            _db.SaveChanges();

            var command = new DeleteTreatmentCommand(treatment.Id);

            // Act

            handler.DeleteTreatmentAsync(command).GetAwaiter().GetResult();

            // Assert
            Assert.That(_db.Treatments.Any(), Is.False);
        }

    }
}
