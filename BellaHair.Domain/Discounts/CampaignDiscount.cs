using BellaHair.Domain.Bookings;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Discounts
{
    public class CampaignDiscount : DiscountBase
    {
        public string Name { get; private set; }
        public DiscountPercent DiscountPercent { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private readonly List<Treatment> _treatments = [];
        public IReadOnlyList<Treatment> Treatments => _treatments.AsReadOnly();


#pragma warning disable CS8618
        private CampaignDiscount() { }
#pragma warning restore CS8618

        private CampaignDiscount(string name, DiscountPercent discountPercent, DateTime startDate, DateTime endDate, IEnumerable<Treatment> treatments)
        {
            if (endDate < startDate)
                throw new CampaignDiscountException("Startdato skal være før slutdato.");

            var treatmentList = treatments.ToList();

            if (treatmentList.Count == 0)
                throw new CampaignDiscountException("Kampagnerabatten skal gælde for mindst én behandling.");

            Id = Guid.NewGuid();
            Name = name;
            DiscountPercent = discountPercent;
            StartDate = startDate;
            EndDate = endDate;
            _treatments = treatmentList;
        }

        public static CampaignDiscount Create(string name, DiscountPercent discountPercent, DateTime startDate, DateTime endDate, IEnumerable<Treatment> treatments) =>
            new(name, discountPercent, startDate, endDate, treatments);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Treatment == null)
                throw new InvalidOperationException("Treatment must be included in booking to calculate discount.");

            if (booking.StartDateTime < StartDate || booking.StartDateTime > EndDate)
                return BookingDiscount.Inactive(Name);

            if (_treatments.All(t => t.Id != booking.Treatment.Id))
                return BookingDiscount.Inactive(Name);

            var discount = booking.Total * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discount);
        }
    }

    public class CampaignDiscountException(string message) : DomainException(message);
}
