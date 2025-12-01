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
        public int Visits { get; private set; }
        public DateTime Birthday { get; private set; }
        private readonly List<Booking> _bookings = [];

        // Den offentlige liste af bookings gøres immutable gennem casting til en IReadOnlyCollection.
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

        private readonly List<int> _birthdayDiscountUsedYears = [];
        public IReadOnlyCollection<int> BirthdayDiscountUsedYears => _birthdayDiscountUsedYears.AsReadOnly();

        private PrivateCustomer() { }

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

        // Ville være mere clean hvis den tog list bookings og ICurrentDateTimeProvider
        // Men så skal vi loade alle bookings ud af DB "bare" for at få dette tal
        public void SetVisits(int visits)
        {
            if (visits < 0) throw new PrivateCustomerException("Antal besøg kan ikke være et negativt tal.");

            Visits = visits;
        }

        // Kunden skal minimum være 18 år gammel
        private static void ValidateBirthday(DateTime birthday, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            if (birthday > currentDateTimeProvider.GetCurrentDateTime().AddYears(-18))
                throw new PrivateCustomerException("Customers must be 18 years of age");
        }

        public bool HasUsedBirthdayDiscount(int year)
        {
            return _birthdayDiscountUsedYears.Contains(year);
        }

        public void RegisterBirthdayDiscountUsed(int year)
        {
            if (_birthdayDiscountUsedYears.Contains(year))
                throw new PrivateCustomerException($"Kunden har allerede brugt fødselsdagsrabat i år {year}.");

            _birthdayDiscountUsedYears.Add(year);
        }


    }

    public class PrivateCustomerException(string message) : DomainException(message);
}
