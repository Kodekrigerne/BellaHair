using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
    //Dennis
    /// <summary>
    /// A percentage discount which is applied to all customers with a minimum amount of visits.
    /// </summary>
    public class LoyaltyDiscount : DiscountBase
    {
        public string Name { get; private init; }
        public int MinimumVisits { get; private init; }
        public DiscountPercent DiscountPercent { get; private init; }

#pragma warning disable CS8618
        private LoyaltyDiscount() { }
#pragma warning restore CS8618

        private LoyaltyDiscount(string discountName, int minimumVisits, DiscountPercent discountPercent)
        {
            Id = Guid.NewGuid();
            Name = discountName;
            MinimumVisits = minimumVisits;
            DiscountPercent = discountPercent;
        }

        public static LoyaltyDiscount Create(string discountName, int minimumVisits, DiscountPercent discountPercent) => new(discountName, minimumVisits, discountPercent);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            //TODO: Uncomment after implementing customer visits counter
            if (booking.Customer.Visits < MinimumVisits) return BookingDiscount.Inactive(Name);

            var discountAmount = booking.Total * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discountAmount);
        }
    }
}
