using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
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
    public class BirthdayDiscountException(string message) : DomainException(message);
}
