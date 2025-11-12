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

            //TPC mapping vælges så alle nedarvere af DiscountBase får deres egen tabel, og ingen tabel for baseklassen oprettes.
            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();
            modelBuilder.Entity<LoyaltyDiscount>().ComplexProperty(l => l.DiscountPercent);
        }
    }
}
