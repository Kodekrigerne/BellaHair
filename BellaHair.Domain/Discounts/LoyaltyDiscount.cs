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
            if (minimumVisits < 1) throw new LoyaltyDiscountException("Minimum visits must be at least 1");

            Id = Guid.NewGuid();
            Name = discountName;
            MinimumVisits = minimumVisits;
            DiscountPercent = discountPercent;
        }

        public static LoyaltyDiscount Create(string discountName, int minimumVisits, DiscountPercent discountPercent) => new(discountName, minimumVisits, discountPercent);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Customer == null) throw new InvalidOperationException("Customer must be included with Booking in order to calculate discount");

            if (booking.Customer.Visits < MinimumVisits) return BookingDiscount.Inactive(Name);

            var discountAmount = booking.TotalBase * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discountAmount);
        }
    }

    public class LoyaltyDiscountException(string message) : DomainException(message);
}
