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
#pragma warning disable CA1862
            return await _db.Treatments
                .AsNoTracking()
                .AnyAsync(t => t.Name.ToLower() == name.ToLower()
                               && t.DurationMinutes.Value == duration);
#pragma warning restore CA1862
        }
    }
}
