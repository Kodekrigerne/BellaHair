    namespace BellaHair.Domain.Employees;
using System.Collections.ObjectModel;
using BellaHair.Domain;
using BellaHair.Domain.SharedValueObjects;

public class Employee : PersonBase
    {

    //private readonly List<Treatment> _treatments; //Inde i
    //public IReadOnlyList<Treatment> Treatments => _treatments.AsReadOnly(); //Udad

#pragma warning disable CS8618
    private Employee() { }
#pragma warning restore CS8618

    private Employee(Name name, Email email, PhoneNumber phoneNumber, Address address /*List<Treatment> treatments*/)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        //_treatments = treatments;
    }

    public static Employee Create(Name name, Email email, PhoneNumber phoneNumber, Address address ) => new(name, email, phoneNumber, address);
    }

public class EmployeeException(string message) : DomainException(message);