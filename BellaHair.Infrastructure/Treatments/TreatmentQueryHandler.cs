using BellaHair.Ports.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Treatments
{
    public class TreatmentQueryHandler : ITreatmentQuery
    {
        private readonly BellaHairContext _db;

        public TreatmentQueryHandler(BellaHairContext db)
            => _db = db;

        async Task<List<TreatmentDTO>> ITreatmentQuery.GetTreatments()
        {
            return await _db.Treatments.AsNoTracking()
                .Select(t => new TreatmentDTO(t.Id, t.Name, t.Price.Value, t.DurationMinutes.Value)).ToListAsync();
        }
    }
}
