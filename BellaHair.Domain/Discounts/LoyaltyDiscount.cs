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
        public override DiscountType Type => DiscountType.LoyaltyDiscount;
        public int MinimumVisits { get; private init; }
        public DiscountPercent TreatmentDiscountPercent { get; private init; }
        public DiscountPercent? ProductDiscountPercent { get; private init; }

#pragma warning disable CS8618
        private LoyaltyDiscount() { }
#pragma warning restore CS8618

        private LoyaltyDiscount(string discountName, int minimumVisits, DiscountPercent treatmentDiscountPercent, DiscountPercent? productDiscountPercent = null)
        {
            if (minimumVisits < 1) throw new LoyaltyDiscountException("Minimum visits must be at least 1");

            Id = Guid.NewGuid();
            Name = discountName;
            MinimumVisits = minimumVisits;
            TreatmentDiscountPercent = treatmentDiscountPercent;
            ProductDiscountPercent = productDiscountPercent;
        }

        public static LoyaltyDiscount Create(string discountName, int minimumVisits, DiscountPercent treatmentDiscountPercent) => new(discountName, minimumVisits, treatmentDiscountPercent);
        public static LoyaltyDiscount CreateWithProductDiscount(string discountName, int minimumVisits, DiscountPercent treatmentDiscountPercent, DiscountPercent productDiscountPercent) => new(discountName, minimumVisits, treatmentDiscountPercent, productDiscountPercent);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Customer == null || booking.Treatment == null || booking.ProductLines == null)
                throw new InvalidOperationException("Relations must be included with Booking in order to calculate discount");

            if (booking.Customer.Visits < MinimumVisits) return BookingDiscount.Inactive(Name, Type);

            var discountAmount = 0m;
            discountAmount += booking.Treatment.Price.Value * TreatmentDiscountPercent.Value;
            if (ProductDiscountPercent != null)
            {
                discountAmount += booking.ProductLines
                    .Sum(pl => pl.Quantity.Value * pl.Product.Price.Value * ProductDiscountPercent.Value);
            }

            return BookingDiscount.Active(Name, discountAmount, Type);
        }
    }

    public class LoyaltyDiscountException(string message) : DomainException(message);
}
