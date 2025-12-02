using BellaHair.Domain.Employees;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Treatments
{
    // Mikkel Klitgaard

    /// <summary>
    /// Represents a domain-entity Treatment with a unique Id, a display Name, a Price and a Duration in minutes.
    /// Instances are immutable after creation; use the static Create factory to construct a Treatment. The factory
    /// (and the private constructor) validates the treatment name to only contain letters, digits and spaces and
    /// throws a TreatmentException when the name is invalid.
    /// </summary>
    public class Treatment : EntityBase
    {
        public string Name { get; private set; }
        public Price Price { get; private set; }
        public DurationMinutes DurationMinutes { get; private set; }
        public IReadOnlyList<Employee>? Employees;

#pragma warning disable CS8618
        public Treatment() { }
#pragma warning restore CS8618

        private Treatment(string treatmentName, Price price, DurationMinutes durationMinutes)
        {
            if (treatmentName.Any(c => !char.IsLetterOrDigit(c) && c != ' '))
                throw new TreatmentException("Behandlingsnavn må kun bestå af bogstaver og tal.");

            Id = Guid.NewGuid();
            Name = treatmentName;
            Price = price;
            DurationMinutes = durationMinutes;
        }

        public static Treatment Create(string treatmentName, Price price, DurationMinutes durationMinutes)
            => new(treatmentName, price, durationMinutes);
    }
    public class TreatmentException(string message) : DomainException(message);
}
