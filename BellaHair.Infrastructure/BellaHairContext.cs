using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure
{
    public class BellaHairContext : DbContext
    {
        public BellaHairContext(DbContextOptions<BellaHairContext> options) : base(options) { }

        public DbSet<DiscountBase> Discounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TPC mapping vælges så alle nedarvere af DiscountBase får deres egen tabel, og ingen tabel for baseklassen oprettes.
            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();

            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.Price);
            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.DurationMinutes);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Name);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Email);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.PhoneNumber);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Address);

            modelBuilder.Entity<LoyaltyDiscount>().ComplexProperty(l => l.DiscountPercent);
        }
    }
}
