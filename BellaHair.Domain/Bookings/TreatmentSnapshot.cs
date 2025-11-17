using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Bookings
{
    public record TreatmentSnapshot
    {
        public Guid TreatmentId { get; private init; }
        public string Name { get; private init; }
        public decimal Price { get; private init; }
        public int DurationMinutes { get; private init; }

#pragma warning disable CS8618
        private TreatmentSnapshot() { }
#pragma warning restore CS8618

        private TreatmentSnapshot(Treatment treatment)
        {
            TreatmentId = treatment.Id;
            Name = treatment.Name;
            Price = treatment.Price.Value;
            DurationMinutes = treatment.DurationMinutes.Value;
        }

        public static TreatmentSnapshot FromTreatment(Treatment treatment) => new(treatment);
    }
}
