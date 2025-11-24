using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Discounts
{
    public class CampaignDiscount : DiscountBase
    {
        public string Name { get; private init; }
        public DiscountPercent DiscountPercent { get; private init; }

#pragma warning disable CS8618
        private CampaignDiscount() {}
#pragma warning restore CS8618

        private CampaignDiscount(string name, DiscountPercent discountPercent)
        {
            Id = Guid.NewGuid();
            Name = name;
            DiscountPercent = discountPercent;
        }

        public override BookingDiscount CalculateBookingDiscount(Booking booking)
        {
            if (booking.Treatment == null)
                throw new InvalidOperationException("Treatment must be included in booking to calculate discount.");

            var discount = booking.Total * DiscountPercent.Value;
            return BookingDiscount.Active(Name, discount);
        }
    }

    public class CampaignDiscountException(string message) : DomainException(message);
}
