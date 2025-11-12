using BellaHair.Domain.SharedValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain
{
    // Mikkel Dahlmann
    /// <summary>
    /// Base class for different kinds of persons.
    /// </summary>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    public abstract class PersonBase : EntityBase
    {
        public Name Name { get; protected set; }
        public Email Email { get; protected set; }
        public PhoneNumber PhoneNumber { get; protected set; }
        public Address Address { get; protected set; }
    }
}
