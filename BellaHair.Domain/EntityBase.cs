namespace BellaHair.Domain
{
    /// <summary>
    /// Base class for all entities.
    /// </summary>
    /// <remarks>
    /// Specifies a Guid Id which must be set in all inheriting classes.
    /// This is left out of this classes constructor to allow each entity to choose a strategy for itself.
    /// </remarks>
    public class EntityBase
    {
        public Guid Id { get; protected init; }
    }
}
