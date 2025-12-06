using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BellaHair.Infrastructure.Tests
{
    // Mikkel Dahlmann

    /// <summary>
    /// Provides an abstract baseclass for integration testclasses in the infrastructurelayer.
    /// Handles setup and disposure of database connection.
    /// Ensures a clean-slate database for each test run.
    /// </summary>

    public abstract class InfrastructureTestBase
    {
        // Sti til skrivebord på afviklende maskine hentes gennem Environment-klassen.
        private static readonly string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly string _dbPath = Path.Combine(_desktopPath, "test.sqlite");
        protected DbContextOptions<BellaHairContext> _options = null!;
        protected BellaHairContext _db;
        protected IServiceProvider ServiceProvider;

        // Setup af dbcontext ved start af test-suite. Gemmer kopi af test-database på maskinens skrivebord.
        // Laver serviceprovider til dependency injection.
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();

            var options = new DbContextOptionsBuilder<BellaHairContext>().UseSqlite($"Data Source={_dbPath}").Options;
            services.AddSingleton(options);
            services.AddDbContext<BellaHairContext>();

            services.AddInfrastructureServices();

            ServiceProvider = services.BuildServiceProvider();

            _db = ServiceProvider.GetRequiredService<BellaHairContext>();
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

            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
