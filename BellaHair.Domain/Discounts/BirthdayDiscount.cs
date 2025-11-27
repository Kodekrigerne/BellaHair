using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.Discounts
{
    public class BirthdayDiscount : DiscountBase
    {
        public string Name { get; private set; }
        public DiscountPercent DiscountPercent { get; private set; }


#pragma warning disable CS8618
        private BirthdayDiscount() {}
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
            throw new NotImplementedException();
        }
    }
    public class BirthdayDiscountException(string message) : DomainException(message);
}
