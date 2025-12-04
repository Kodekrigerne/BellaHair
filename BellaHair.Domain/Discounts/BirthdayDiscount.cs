using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
    // Mikkel Klitgaard
    /// <summary>
    /// Represents a discount that is applied to a booking when the customer's birthday occurs in the same month as the
    /// booking date.
    /// </summary>
    /// <remarks>A birthday discount is only active if the booking's customer has not already used a birthday
    /// discount in the same year and the booking occurs in the customer's birthday month. The discount percentage and
    /// name are specified when creating the discount. This type is typically used to provide special offers to
    /// customers during their birthday month.</remarks>
    public class BirthdayDiscount : DiscountBase
    {
        public string Name { get; private set; }
        public override DiscountType Type => DiscountType.BirthdayDiscount;
        public DiscountPercent DiscountPercent { get; private set; }


#pragma warning disable CS8618
        private BirthdayDiscount() { }
#pragma warning restore CS8618

        private BirthdayDiscount(string name, DiscountPercent discountPercent)
        {
            Id = Guid.NewGuid();
            Name = name;
            DiscountPercent = discountPercent;
        }

        public static BirthdayDiscount Create(string discountName, DiscountPercent discountPercent)
            => new(discountName, discountPercent);


        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Customer == null) 
                throw new InvalidOperationException("Customer must be included with Booking in order to calculate discount");
            if (booking.Treatment == null)
                throw new InvalidOperationException("Treatment must be included with Booking in order to calculate discount");

            if (booking.StartDateTime.Month != booking.Customer.Birthday.Month)
                return BookingDiscount.Inactive(Name, Type);

            if (booking.Customer.HasUsedBirthdayDiscount(booking.StartDateTime.Year))
                return BookingDiscount.Inactive(Name, Type);

            var discount = booking.Treatment.Price.Value * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discount, Type);
        }
    }
}
