using BellaHair.Application;
using Microsoft.EntityFrameworkCore.Storage;

namespace BellaHair.Infrastructure
{
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
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            _currentTransaction?.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
