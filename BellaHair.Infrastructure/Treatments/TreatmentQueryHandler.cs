using BellaHair.Ports.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Treatments
{
    // Mikkel Klitgaard
    /// <summary>
    /// Handles queries related to treatments by retrieving treatment data from the database.
    /// </summary>
    /// <remarks>This class implements the <see cref="ITreatmentQuery"/> interface and provides functionality
    /// to query treatment information, such as retrieving a list of treatments. It uses the <see
    /// cref="BellaHairContext"/> to interact with the database.</remarks>
    public class TreatmentQueryHandler : ITreatmentQuery
    {
        private readonly BellaHairContext _db;

        public TreatmentQueryHandler(BellaHairContext db)
            => _db = db;

        async Task<List<TreatmentDTO>> ITreatmentQuery.GetAllAsync()
        {
            return await _db.Treatments.AsNoTracking()
                .Select(t => new TreatmentDTO(
                    t.Id,
                    t.Name,
                    t.Price.Value,
                    t.DurationMinutes.Value,
                    t.Employees.Count()))
                .ToListAsync();
        }
    }
}
