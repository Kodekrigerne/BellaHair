using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CrossCut
{
    public class DbConfigure
    {
        private readonly BellaHairContext _db;

        public DbConfigure(BellaHairContext db) => _db = db;

        public void ConfigureDb()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            _db.Database.ExecuteSqlRaw("PRAGMA journal_mode=DELETE;");
        }
    }
}
