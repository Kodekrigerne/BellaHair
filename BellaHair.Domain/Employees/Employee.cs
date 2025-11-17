    namespace BellaHair.Domain.Employees;
using System.Collections.ObjectModel;
using BellaHair.Domain;
using BellaHair.Domain.SharedValueObjects;
using BellaHair.Domain.Treatments;

public class Employee : PersonBase
    {

    private readonly List<Treatment> _treatments; //Inde i
    public IReadOnlyList<Treatment> Treatments => _treatments.AsReadOnly(); //Udad

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
    }

    public static Employee Create(Name name, Email email, PhoneNumber phoneNumber, Address address, List<Treatment> treatments) => new(name, email, phoneNumber, address, treatments);
    }

public class EmployeeException(string message) : DomainException(message);