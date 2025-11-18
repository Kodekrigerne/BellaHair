using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Tests.Treatments
{
    // Mikkel Klitgaard

    internal sealed class TreatmentTests
    {
        [TestCase("Beh@ndling", 450, 120)]
        [TestCase("Behandling#¤%", 450, 120)]
        [TestCase("()", 450, 120)]
        public void Given_InvalidTreatmentName_Then_ThrowsException(string name, decimal price, int duration)
        {
            // Arrange
            var invalidName = name;
            var validPrice = Price.FromDecimal(price);
            var validDuration = DurationMinutes.FromInt(duration);

            // Act & Assert
            Assert.Throws<TreatmentException>(() =>
                Treatment.Create(invalidName, validPrice, validDuration));
        }

        [TestCase("Behandling", 450, 120)]
        [TestCase("Behandling 1", 450, 120)]
        [TestCase("12345", 450, 120)]
        public void Given_ValidTreatment_Then_CreatesTreatment(string name, decimal price, int duration)
        {
            // Arrange
            var validName = name;
            var validPrice = Price.FromDecimal(price);
            var validDuration = DurationMinutes.FromInt(duration);

            // Act
            var treatment = Treatment.Create(validName, validPrice, validDuration);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(treatment.Name, Is.EqualTo(validName));
                Assert.That(treatment.Price.Value, Is.EqualTo(validPrice.Value));
                Assert.That(treatment.DurationMinutes.Value, Is.EqualTo(validDuration.Value));
            });
        }
    }
}
