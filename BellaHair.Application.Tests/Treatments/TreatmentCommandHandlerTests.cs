using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Ports.Treatments;
using FixtureBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Application.Tests.Treatments
{
    // Mikkel Klitgaard
    /// <summary>
    /// 
    /// </summary>
    internal sealed class TreatmentCommandHandlerTests : ApplicationTestBase
    {
        [Test]
        public async Task Given_Treatment_Then_CreatesTreatment()
        {
            // Arrange

            //var repo = (ITreatmentRepository)new TreatmentRepository(_db);
            //var dateTimeProvider = (ICurrentDateTimeProvider)new CurrentDateTimeProvider();
            //var bookingChecker = (IFutureBookingWithTreatmentChecker)new FutureBookingWithTreatmentChecker(_db, dateTimeProvider);
            //var handler = (ITreatmentCommand)new TreatmentCommandHandler(repo, bookingChecker);

            //Nu bruger vi en ServiceProvider (IoC) istedet for, som sørger for at alle services som ITreatmentCommand skal bruge, bliver injiceret.
            var handler = ServiceProvider.GetRequiredService<ITreatmentCommand>();
            var command = new CreateTreatmentCommand("Herreklip", 450m, 45);

            // Act

            await handler.CreateTreatmentAsync(command);
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
        public async Task Given_Treatment_Then_DeletesTreatment()
        {
            // Arrange

            var handler = ServiceProvider.GetRequiredService<ITreatmentCommand>();
            var treatment = Treatment.Create("Herreklip", Price.FromDecimal(450), DurationMinutes.FromInt(45));

            _db.Add(treatment);
            _db.SaveChanges();

            var command = new DeleteTreatmentCommand(treatment.Id);

            // Act

            await handler.DeleteTreatmentAsync(command);

            // Assert
            Assert.That(_db.Treatments.Any(), Is.False);
        }

        [Test]
        public async Task Given_DeleteTreatmentWithFutureBookings_Then_ThrowsException()
        {
            // Arrange
            var handler = ServiceProvider.GetRequiredService<ITreatmentCommand>();
            var treatment = Treatment.Create("Voksbehandling", Price.FromDecimal(300), DurationMinutes.FromInt(30));

            var bookingFixture = Fixture.New<Booking>().UseConstructor()
                                        .WithSetter(b => b.StartDateTime, DateTime.Now.AddDays(1))
                                        .WithSetter(b => b.Treatment, treatment)
                                        .Build();

            // Act
            await _db.AddAsync(treatment);
            await _db.Bookings.AddAsync(bookingFixture);
            await _db.SaveChangesAsync();
            var deleteCommand = new DeleteTreatmentCommand(treatment.Id);

            // Assert 
          Assert.ThrowsAsync<DomainException>( async () => await handler.DeleteTreatmentAsync(deleteCommand));
        }

    }
}
