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

        public static LoyaltyDiscount Create(string discountName, int minimumVisits, DiscountPercent discountPercent) => new(discountName, minimumVisits, discountPercent);
        public static LoyaltyDiscount CreateWithProductDiscount(string discountName, int minimumVisits, DiscountPercent discountPercent, DiscountPercent productDiscount) => new(discountName, minimumVisits, discountPercent, productDiscount);

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Customer == null) throw new InvalidOperationException("Customer must be included with Booking in order to calculate discount");
            if (booking.Treatment == null) throw new InvalidOperationException("Treatment must be included with Booking in order to calculate discount");

            if (booking.Customer.Visits < MinimumVisits) return BookingDiscount.Inactive(Name, Type);

            // Tilføj rabat på behandling
            var discountAmount = booking.Treatment.Price.Value * TreatmentDiscountPercent.Value;

            // Tilføj rabat på produkter
            if (ProductDiscountPercent != null)
            {
                foreach (var productLine in booking.ProductLines)
                {
                    discountAmount += productLine.Quantity.Value * productLine.Product.Price.Value;
                }
            }

            return BookingDiscount.Active(Name, discountAmount, Type);
        }
    }

    public class LoyaltyDiscountException(string message) : DomainException(message);
}
