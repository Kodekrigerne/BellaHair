using BellaHair.Domain.Treatments;

namespace BellaHair.Infrastructure.Treatments
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly BellaHairContext _db;

        public TreatmentRepository(BellaHairContext db)
        {
            _db = db;
        }

        async Task ITreatmentRepository.AddAsync(Treatment treatment)
        {
            await _db.Treatments.AddAsync(treatment);
        }

        void ITreatmentRepository.Delete(Treatment treatment)
        {
            throw new NotImplementedException();
        }

        async Task<Treatment> ITreatmentRepository.Get(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task ITreatmentRepository.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
