using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure.Treatments;

namespace BellaHair.Infrastructure.Tests.Treatments
{
    // Mikkel Klitgaard

    internal sealed class TreatmentRepositoryTests : InfrastructureTestBase
    {
        [Test]
        public void Given_NewTreatment_Then_AddsTreatment()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);

            var treatment = Treatment.Create("Herreklip",
                Price.FromDecimal(250m), DurationMinutes.FromInt(45));

            // Act

            repo.AddAsync(treatment);
            _db.SaveChangesAsync();

            // Assert

            var actualTreatment = _db.Treatments.First();
            Assert.That(actualTreatment.Id, Is.EqualTo(treatment.Id));
        }


        [Test]
        public void Given_TreatmentExists_Then_DeletesTreatment()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);

            var treatment = Treatment.Create("Herreklip",
                Price.FromDecimal(250m), DurationMinutes.FromInt(45));

            _db.Add(treatment);
            _db.SaveChangesAsync();

            // Act
            var treatmentFromDb = _db.Treatments.First();
            repo.Delete(treatmentFromDb);

            _db.SaveChangesAsync();

            // Assert

            Assert.That(_db.Treatments, Is.Empty);
        }

        [Test]
        public void Given_TreatmentExists_Then_GetsTreatmentById()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);

            var treatment = Treatment.Create("Herreklip",
                Price.FromDecimal(250m), DurationMinutes.FromInt(45));

            _db.Add(treatment);
            _db.SaveChangesAsync();

            // Act

            var returnedTreatment = repo.GetAsync(treatment.Id).GetAwaiter().GetResult();

            // Assert

            Assert.That(returnedTreatment.Id, Is.EqualTo(treatment.Id));
        }

        [Test]
        public void Given_InvalidTreatmentId_Then_ThrowsException()
        {
            // Arrange

            var repo = (ITreatmentRepository)new TreatmentRepository(_db);

            var treatment = Treatment.Create("Herreklip",
                Price.FromDecimal(250m), DurationMinutes.FromInt(45));

            // Act

            _db.Add(treatment);
            _db.SaveChangesAsync();

            // Assert

            Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(Guid.NewGuid()));
        }
    }



}
