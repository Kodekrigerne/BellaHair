namespace BellaHair.Domain.Treatments;

// Mikkel Klitgaard
/// <summary>
/// Defines a repository for managing <see cref="Treatment"/> entities, providing methods for adding, retrieving,
/// deleting, and persisting changes.
/// </summary>
/// <remarks>This interface abstracts the data access layer for <see cref="Treatment"/> entities, enabling
/// dependency injection and testability. Implementations of this interface should ensure thread safety and proper
/// handling of asynchronous operations.</remarks>
public interface ITreatmentRepository
{
    Task AddAsync(Treatment treatment);
    void Delete(Treatment treatment);
    Task<Treatment> Get(Guid id);
    Task<IEnumerable<Treatment>> Get(IEnumerable<Guid> treatmentIds);
    Task SaveChangesAsync();
}