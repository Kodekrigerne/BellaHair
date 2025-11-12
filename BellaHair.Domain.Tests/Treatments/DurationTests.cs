using BellaHair.Domain.Treatments.Value_Objects;

namespace BellaHair.Domain.Tests.Treatments
{
    //Mikkel Klitgaard
    internal sealed class DurationTests
    {
        [TestCase(9)]
        [TestCase(0)]
        [TestCase(-1)]
        public void GivenDurationIsLessThan10_Then_ThrowsException(int value)
        {
            // Act & Assert
            Assert.Throws<DurationException>(() => Duration.FromInt(value));
        }

        [TestCase(301)]
        [TestCase(2000)]
        public void GivenDurationIsGreaterThan300_Then_ThrowsException(int value)
        {
            // Act & Assert
            Assert.Throws<DurationException>(() => Duration.FromInt(value));
        }

        [TestCase(10)]
        [TestCase(300)]
        [TestCase(120)]
        public void GivenDurationIsValid_Then_ConstructsDuration(int value)
        {
            // Act
            Duration validDuration = Duration.FromInt(value);

            // Assert
            Assert.That(validDuration.Value, Is.EqualTo(value));
        }
    }
}
