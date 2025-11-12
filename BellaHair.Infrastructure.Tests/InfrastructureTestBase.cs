using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Tests
{
    public abstract class InfrastructureTestBase
    {
        private DbContextOptions<BellaHairContext> _options;
        private BellaHairContext _db;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _options = new DbContextOptionsBuilder<BellaHairContext>().UseSqlite("Data Source=\"C:\"").Options;
            _db = new BellaHairContext(_options);
            _db.Database.OpenConnection();
            _db.Database.EnsureCreated();
        }

        [SetUp]
        public void SetUp()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _db.Dispose();
        }
    }
}
