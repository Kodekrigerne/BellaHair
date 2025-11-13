namespace BellaHair.Ports.Treatments;

// Mikkel Klitgaard
/// <summary>
/// Defines a contract for querying treatment data.
/// </summary>
/// <remarks>This interface provides a method to retrieve a collection of treatments. Implementations of this
/// interface should handle the data retrieval logic, such as accessing a database or an external service.</remarks>
public interface ITreatmentQuery
{
    Task<List<TreatmentDTO>> GetTreatments();
}

public record TreatmentDTO(Guid Id, string Name, decimal Price, int Duration);