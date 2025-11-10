using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure
{
    public class BellaHairContext : DbContext
    {
        public BellaHairContext(DbContextOptions<BellaHairContext> options) : base(options) { }

        public DbSet<DiscountBase> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();

            modelBuilder.Entity<Booking>().ComplexProperty(b => b.Discount);
        }
    }
}
