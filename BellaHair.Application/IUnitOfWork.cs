// Mikkel Dahlmann

namespace BellaHair.Application
{

    /// <summary>
    /// Defines a contract for coordinating the writing of changes and managing transactions across multiple
    /// repositories as a single unit of work.
    /// </summary>

    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
