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
        //TODO: Vælg en strategi
        // 1. .Include(c => c.Bookings) Men loader alle bookings ind sammen med kunde,
        //      kan være overkill hvis vi kun skal bruge count. Hvis bookings skal med uanset er det fint.
        // 2. Flyt Visits ud af entitet, udregn den udenfor (i query) og send med i DTO (så den findes I DTO men ikke entitet)
        // 3. Database computed value, burde være simpelt, men ingen kontrol over nutid
        // 4. GetVisits(ICustomerVisitsCalculator _) metode
        public int Visits => Bookings.Count;
        public DateTime Birthday { get; private set; }
        private readonly List<Booking> _bookings = [];

        // Den offentlige liste af bookings gøres immutable gennem casting til en IReadOnlyCollection.
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

#pragma warning disable CS8618
        private PrivateCustomer() { }
#pragma warning restore CS8618


        private PrivateCustomer(Name name, Address address, PhoneNumber phoneNumber, Email email, DateTime birthday)
        {
            ValidateBirthday(birthday);

            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
            _bookings = [];
        }

        internal static PrivateCustomer Create(Name name, Address address, PhoneNumber phoneNumber, Email email, DateTime birthday)
        {
            return new PrivateCustomer(name, address, phoneNumber, email, birthday);
        }

        internal void Update(Name name, Address address, PhoneNumber phoneNumber, Email email, DateTime birthday)
        {
            ValidateBirthday(birthday);

            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
        }

        // Kunden skal minimum være 18 år gammel.
        private static void ValidateBirthday(DateTime birthday)
        {
            if (birthday > DateTime.Now.AddYears(-18))
                throw new PrivateCustomerException("Customers must be 18 years of age");
        }
    }

    public class PrivateCustomerException(string message) : DomainException(message);
}
