using BellaHair.Application;
using Microsoft.EntityFrameworkCore.Storage;

// Mikkel Dahlmann

namespace BellaHair.Infrastructure
{

    /// <summary>
    /// Provides a unit of work implementation for managing database transactions and saving changes within a single
    /// context.
    /// </summary>

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BellaHairContext _db;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(BellaHairContext db) => _db = db;

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            ValidateTransactionBegun();

            try
            {
                await SaveChangesAsync();
                await _currentTransaction!.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                await _currentTransaction!.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            ValidateTransactionBegun();

            await _currentTransaction!.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        private void ValidateTransactionBegun()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Must begin transaction before other operations");
        }
    }
}
