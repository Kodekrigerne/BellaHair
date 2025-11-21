using System.Collections.ObjectModel;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;

namespace BellaHair.Domain.PrivateCustomers
{
    // Mikkel Dahlmann

    /// <summary>
    /// Represents a private customer with personal information and booking history.
    /// </summary>

    public class PrivateCustomer : PersonBase
    {
        public int Visits => _bookings.Count;
        public DateTime Birthday { get; private set; }
        private readonly List<Booking> _bookings = [];

        // Den offentlige liste af bookings gøres immutable gennem casting til en IReadOnlyCollection.
        public IReadOnlyCollection<Booking> Bookings => _bookings?.AsReadOnly();

        #pragma warning disable CS8618
        private PrivateCustomer() { }
        #pragma warning restore CS8618


        private PrivateCustomer(Name name, Address address, PhoneNumber phoneNumber, 
            Email email, DateTime birthday, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            ValidateBirthday(birthday, currentDateTimeProvider);

            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
            _bookings = [];
        }

        public static PrivateCustomer Create(Name name, Address address, PhoneNumber phoneNumber, 
            Email email, DateTime birthday, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            return new PrivateCustomer(name, address, phoneNumber, email, birthday, currentDateTimeProvider);
        }

        public void Update(Name name, Address address, PhoneNumber phoneNumber, 
            Email email, DateTime birthday, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            ValidateBirthday(birthday, currentDateTimeProvider);

            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
        }

        // Kunden skal minimum være 18 år gammel
        private static void ValidateBirthday(DateTime birthday, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            if (birthday > currentDateTimeProvider.GetCurrentDateTime().AddYears(-18))
                throw new PrivateCustomerException("Customers must be 18 years of age");
        }
    }

    public class PrivateCustomerException(string message) : DomainException(message);
}
