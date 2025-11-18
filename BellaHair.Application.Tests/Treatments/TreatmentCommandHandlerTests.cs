using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Application.Treatments;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
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
            var handler = (ITreatmentCommand)new TreatmentCommandHandler(repo);
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
            var handler = (ITreatmentCommand)new TreatmentCommandHandler(repo);
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
