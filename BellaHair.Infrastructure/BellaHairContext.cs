using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.PrivateCustomers;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure
{
    public class BellaHairContext : DbContext
    {
        public BellaHairContext(DbContextOptions<BellaHairContext> options) : base(options) { }

        public DbSet<DiscountBase> Discounts { get; set; }
        public DbSet<PrivateCustomer> PrivateCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TPC mapping vælges så alle nedarvere af DiscountBase får deres egen tabel, og ingen tabel for baseklassen oprettes.
            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();

            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Address);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Name);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Email);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.PhoneNumber);
            
            modelBuilder.Entity<LoyaltyDiscount>().ComplexProperty(l => l.DiscountPercent);

            modelBuilder.Entity<Booking>().ComplexProperty(b => b.Discount, b => b.IsRequired());
        }
    }
}
