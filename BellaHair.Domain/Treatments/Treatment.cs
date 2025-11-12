using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments.ValueObjects;

namespace BellaHair.Domain.Treatments
{
    public class Treatment : EntityBase
    {
        public string Name { get; private set; }
        public Price Price { get; private set; }
        public Duration Duration { get; private set; }

#pragma warning disable CS8618
        public Treatment() { }
#pragma warning restore CS8618

        private Treatment(string treatmentName, Price price, Duration duration)
        {
            if (treatmentName.Any(c => !char.IsLetterOrDigit(c) || c != ' '))
                throw new TreatmentException("Name of treatment should only consist of letters or numbers");

            Id = Guid.NewGuid();
            Name = treatmentName;
            Price = price;
            Duration = duration;
        }

        public static Treatment Create(string treatmentName, Price price, Duration duration) => new(treatmentName, price, duration);
    }
    public class TreatmentException(string message) : DomainException(message);
}
