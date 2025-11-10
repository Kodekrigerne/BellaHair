using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure.Tests
{
    internal class BellaHairContextTests
    {
        [Test]
        public void CanConstruct()
        {
            var options = new DbContextOptionsBuilder<BellaHairContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            Assert.DoesNotThrow(() => new BellaHairContext(options));
        }
    }
}
