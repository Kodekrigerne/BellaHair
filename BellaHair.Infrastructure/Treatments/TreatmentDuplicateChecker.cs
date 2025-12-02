using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .AnyAsync(t => t.Name.ToLower() == name.ToLower()
                               && t.DurationMinutes.Value == duration);
        }
    }
}
