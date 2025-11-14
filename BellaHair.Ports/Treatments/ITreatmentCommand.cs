namespace BellaHair.Ports.Treatments;

// Mikkel Klitgaard
/// <summary>
/// Defines a contract for executing treatment-related commands.
/// </summary>
/// <remarks>This interface provides methods for creating and deleting treatments.  Implementations of this
/// interface should handle the specific logic for processing these commands.</remarks>
public interface ITreatmentCommand
{
    Task CreateTreatmentAsync(CreateTreatmentCommand command);
    Task DeleteTreatmentAsync(DeleteTreatmentCommand command);
}

public record DeleteTreatmentCommand(Guid Id);
public record CreateTreatmentCommand(string Name, decimal Price, int Duration);

