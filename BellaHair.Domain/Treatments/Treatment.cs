using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Treatments
{
    public class Treatment : EntityBase
    {
        public string Name { get; private set; }
        public Price Price { get; private set; }
        public DurationMinutes DurationMinutes { get; private set; }

#pragma warning disable CS8618
        public Treatment() { }
#pragma warning restore CS8618

        private Treatment(string treatmentName, Price price, DurationMinutes durationMinutes)
        {
            if (treatmentName.Any(c => !char.IsLetterOrDigit(c) || c != ' '))
                throw new TreatmentException("Name of treatment should only consist of letters or numbers");

            Id = Guid.NewGuid();
            Name = treatmentName;
            Price = price;
            DurationMinutes = durationMinutes;
        }

        public static Treatment Create(string treatmentName, Price price, DurationMinutes durationMinutes) => new(treatmentName, price, durationMinutes);
    }
    public class TreatmentException(string message) : DomainException(message);
}
