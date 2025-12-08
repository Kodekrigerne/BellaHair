using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BellaHair.Domain.Treatments
{
    // Mikkel Klitgaard
    /// <summary>
    /// Defines a contract for checking whether a treatment with the specified name and duration is considered a
    /// duplicate.
    /// </summary>

    public interface ITreatmentDuplicateChecker
    {
        Task<bool> IsDuplicateAsync(string name, int duration);
    }
    public class TreatmentDuplicateException(string message) : DomainException(message);
}
