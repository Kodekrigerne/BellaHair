using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
    public class BirthdayDiscount : DiscountBase
    {
        public string Name { get; private set; }
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
            if (booking.StartDateTime.Month != booking.Customer.Birthday.Month)
                return BookingDiscount.Inactive(Name);

            if (booking.Customer.HasUsedBirthdayDiscount(booking.StartDateTime.Year))
                return BookingDiscount.Inactive(Name);

            var discount = booking.Total * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discount);
        }
    }
    public class BirthdayDiscountException(string message) : DomainException(message);
}
