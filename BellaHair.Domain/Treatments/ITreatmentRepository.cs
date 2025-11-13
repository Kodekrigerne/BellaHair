namespace BellaHair.Domain.Treatments;

public interface ITreatmentRepository
{
    Task AddAsync(Treatment treatment);
    void Delete(Treatment treatment);
    Task<Treatment> Get(Guid id);
    Task SaveChangesAsync();
}