using BellaHair.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Application.Tests
{
    // Mikkel Dahlmann

    /// <summary>
    /// Provides an abstract baseclass for integration testclasses in the applicationlayer.
    /// Handles setup and disposure of database connection.
    /// Ensures a clean-slate database for each test run.
    /// </summary>
   
    public abstract class ApplicationTestBase
    {
        private DbContextOptions<BellaHairContext> _options;
        private BellaHairContext _db;

        // Setup af dbcontext ved start af test-suite. Gemmer kopi af test-database på C-drevet.
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _options = new DbContextOptionsBuilder<BellaHairContext>().UseSqlite("Data Source=\"C:\"").Options;
            _db = new BellaHairContext(_options);
            _db.Database.OpenConnection();
            _db.Database.EnsureCreated();
        }

        // Setup ved hver kørte test. Sletter indholdet af database-filen, så testen kører på clean-slate.
        [SetUp]
        public void SetUp()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        // Afvikler forbindelsen til databasen ved test-suitens afslutning.
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _db.Database.CloseConnection();
            _db.Dispose();
        }
    }
}