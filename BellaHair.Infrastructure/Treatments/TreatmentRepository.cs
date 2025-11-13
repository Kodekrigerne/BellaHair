using BellaHair.Domain.Treatments;

namespace BellaHair.Infrastructure.Treatments
{
    // Mikkel Klitgaard
    /// <summary>
    /// Provides methods for managing treatments in the BellaHair database.
    /// </summary>
    /// <remarks>This repository implements the <see cref="ITreatmentRepository"/> interface and provides
    /// functionality for adding, deleting, retrieving, and saving treatments. It interacts with the underlying database
    /// context <see cref="BellaHairContext"/> to perform these operations.</remarks>
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly BellaHairContext _db;

        public TreatmentRepository(BellaHairContext db)
            => _db = db;

        async Task ITreatmentRepository.AddAsync(Treatment treatment)
        {
            await _db.Treatments.AddAsync(treatment);
        }

        void ITreatmentRepository.Delete(Treatment treatment)
        {
            _db.Remove(treatment);
        }

        async Task<Treatment> ITreatmentRepository.Get(Guid id)
        {
            return await _db.Treatments.FindAsync(id) ??
                   throw new KeyNotFoundException($"Treatment with id {id} is not found.");
        }

        async Task ITreatmentRepository.SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
