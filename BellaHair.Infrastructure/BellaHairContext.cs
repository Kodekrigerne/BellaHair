using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure
{
    public class BellaHairContext : DbContext
    {
        public BellaHairContext(DbContextOptions<BellaHairContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


        }
    }
}
