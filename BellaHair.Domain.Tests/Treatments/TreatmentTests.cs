using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Tests.Treatments
{
    internal sealed class TreatmentTests
    {
        [Test]
        public void GivenInvalidTreatment_Then_ThrowsException()
        {
            // Arrange
            var name = "Beh@ndling";
            var price = Price.FromDecimal(450m);
            var duration = Duration.FromInt(120);

            // Act
            var invalidTreatment = Treatment.Create(name, price, duration);

            // Assert

        }

        [Test]
        public void GivenValidTreatment_Then_CreatesTreatment()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
