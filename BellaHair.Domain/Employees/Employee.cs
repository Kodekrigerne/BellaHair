namespace BellaHair.Domain.Employees;
using BellaHair.Domain;
using BellaHair.Domain.Bookings;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;

// Linnea

public class Employee : PersonBase
{

    private readonly List<Treatment> _treatments; //Inde i
    public IReadOnlyList<Treatment> Treatments => _treatments.AsReadOnly(); //Udad

    private readonly List<Booking> _bookings;
    public IReadOnlyList<Booking> Bookings => _bookings.AsReadOnly();

#pragma warning disable CS8618
    private Employee() { }
#pragma warning restore CS8618

    private Employee(Name name, Email email, PhoneNumber phoneNumber, Address address, IEnumerable<Treatment> treatments)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        _treatments = treatments.ToList();
        _bookings = [];
    }

    public static Employee Create(Name name, Email email, PhoneNumber phoneNumber, Address address, List<Treatment> treatments) => new(name, email, phoneNumber, address, treatments);

    public void Update(Name name, Email email, PhoneNumber phoneNumber, Address address, List<Treatment> treatments)
    {
        Name = name;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        _treatments.Clear();
        _treatments.AddRange(treatments);
    }

}

public class EmployeeException(string message) : DomainException(message);
