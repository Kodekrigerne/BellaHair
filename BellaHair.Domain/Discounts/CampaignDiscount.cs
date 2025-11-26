using BellaHair.Domain.Bookings;
using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Represents a discount campaign that applies a percentage-based discount to specified treatments within a defined
    /// date range.
    /// </summary>
    /// <remarks>A campaign discount is associated with one or more treatments and is only active between the
    /// specified start and end dates. The discount is applied to bookings that match the campaign's treatments and fall
    /// within the campaign period. Use the <see cref="Create"/> method to instantiate a campaign discount with the
    /// required parameters.</remarks>
    public class CampaignDiscount : DiscountBase
    {
        public string Name { get; private set; }
        public DiscountPercent DiscountPercent { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public  List<Guid> TreatmentIds { get; private set; } = [];


#pragma warning disable CS8618
        private CampaignDiscount() { }
#pragma warning restore CS8618

        private CampaignDiscount(string discountName, DiscountPercent discountPercent, DateTime startDate, DateTime endDate, IEnumerable<Guid> treatments)
        {
            var treatmentList = treatments.ToList();

            if (endDate < startDate)
                throw new CampaignDiscountException("Startdato skal være før slutdato.");

            if (treatmentList.Count == 0)
                throw new CampaignDiscountException("Kampagnerabatten skal gælde for mindst én behandling.");

            Id = Guid.NewGuid();
            Name = discountName;
            DiscountPercent = discountPercent;
            StartDate = startDate;
            EndDate = endDate;
            TreatmentIds = treatmentList;
        }

        public static CampaignDiscount Create(string discountName, DiscountPercent discountPercent, DateTime startDate, DateTime endDate, IEnumerable<Guid> treatmentIds) =>
            new(discountName, discountPercent, startDate, endDate, treatmentIds);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Treatment == null)
                throw new InvalidOperationException("Treatment must be included in booking to calculate discount.");

            if (booking.StartDateTime < StartDate || booking.StartDateTime > EndDate)
                return BookingDiscount.Inactive(Name);

            if (!TreatmentIds.Contains(booking.Treatment.Id))
                return BookingDiscount.Inactive(Name);

            var discount = booking.Total * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discount);
        }
    }

    public class CampaignDiscountException(string message) : DomainException(message);
}
