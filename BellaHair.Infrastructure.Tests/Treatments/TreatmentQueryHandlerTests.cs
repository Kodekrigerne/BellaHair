using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;
using BellaHair.Infrastructure.Treatments;
using BellaHair.Ports.Treatments;

namespace BellaHair.Infrastructure.Tests.Treatments
{
    // Mikkel Klitgaard

    internal sealed class TreatmentQueryHandlerTests :InfrastructureTestBase
    {
        [Test]
        public void Given_TreatmentsExists_Then_GetsListOfAllTreatments()
        {
            // Arrange

            var handler = (ITreatmentQuery)new TreatmentQueryHandler(_db);

            var treatment1 = Treatment.Create("Herreklip", 
                Price.FromDecimal(250m), DurationMinutes.FromInt(45));

            var treatment2 = Treatment.Create("Hårfarvning", 
                Price.FromDecimal(800m), DurationMinutes.FromInt(120));

            var treatment3 = Treatment.Create("Dameklip", 
                Price.FromDecimal(500m), DurationMinutes.FromInt(60));

            _db.AddRange(treatment1, treatment2, treatment3);
            _db.SaveChanges();

            // Act

            var treatmentsList = handler.GetAllAsync().GetAwaiter().GetResult();

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(treatmentsList, Has.Count.EqualTo(3));
                Assert.That(treatmentsList.Any(t => t.Id == treatment1.Id), Is.True);
                Assert.That(treatmentsList.Any(t => t.Id == treatment2.Id), Is.True);
                Assert.That(treatmentsList.Any(t => t.Id == treatment3.Id), Is.True);
            });
        }
    }
}
