using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.PrivateCustomers;
using BellaHair.Domain.Treatments;
using Microsoft.EntityFrameworkCore;

namespace BellaHair.Infrastructure
{
    public class BellaHairContext : DbContext
    {
        public BellaHairContext(DbContextOptions<BellaHairContext> options) : base(options) { }

        public DbSet<DiscountBase> Discounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PrivateCustomer> PrivateCustomers { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TPC mapping vælges så alle nedarvere af DiscountBase får deres egen tabel, og ingen tabel for baseklassen oprettes.
            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();

            modelBuilder.Entity<Booking>().OwnsOne(b => b.Discount);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.EmployeeSnapshot);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.CustomerSnapshot);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.TreatmentSnapshot);

            //Vi ignorerer Total da den ikke har nogen setter men istedet har et backing field
            modelBuilder.Entity<Booking>().Ignore(b => b.Total);
            //Vi mapper backing fieldet i stedet for propertien
            modelBuilder.Entity<Booking>().Property<decimal?>("_total")
                .HasColumnName("Total")
                .IsRequired(false);

            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.Price);
            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.DurationMinutes);

            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Name);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Email);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.PhoneNumber);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Address);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Treatments) //  Employee can be associated with many Treatment entities
                .WithMany(); // Treatment entity can also be associated with many Employee entities.

            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Name);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Email);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.PhoneNumber);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Address);

            modelBuilder.Entity<LoyaltyDiscount>().ComplexProperty(l => l.DiscountPercent);
        }
    }
}
