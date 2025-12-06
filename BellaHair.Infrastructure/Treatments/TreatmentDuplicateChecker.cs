using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Treatments
{
    public class TreatmentDuplicateChecker : ITreatmentDuplicateChecker
    {
        private readonly BellaHairContext _db;

        public TreatmentDuplicateChecker(BellaHairContext db)
            => _db = db;

        public async Task<bool> IsDuplicateAsync(string name, int duration)
        {
            return await _db.Treatments
                .AsNoTracking()
                .AnyAsync(t => t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                               && t.DurationMinutes.Value == duration);
        }
    }
}
