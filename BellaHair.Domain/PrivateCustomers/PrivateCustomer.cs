using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.PrivateCustomers
{
    // Mikkel Dahlmann
    public class PrivateCustomer : PersonBase
    {
        //Udregn denne ud fra mængden af fuldførte bookings, get => Bookings.Count ?? Eller som DB computed value
        public int Visits => Bookings.Count;
        public DateTime Birthday { get; private set; }
        private readonly List<Booking> _bookings;
        
        // Den offentlige liste af bookings gøres immutable gennem casting til en IReadOnlyCollection.
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

        private PrivateCustomer(Name name, Address address, PhoneNumber phoneNumber, Email email, DateTime birthday)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
            _bookings = [];
        }

        public static PrivateCustomer Create(Name name, Address address, PhoneNumber phoneNumber, Email email, DateTime birthday)
        {
            return new PrivateCustomer(name, address, phoneNumber, email, birthday);
        }
    }
}
