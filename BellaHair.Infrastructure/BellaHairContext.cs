using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Employees;
using BellaHair.Domain.Invoices;
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
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TPC mapping vælges så alle nedarvere af DiscountBase får deres egen tabel, og ingen tabel for baseklassen oprettes.
            modelBuilder.Entity<DiscountBase>().UseTpcMappingStrategy();

            //.OwnsOne er nødvendigt da .ComplexProperty endnu ikke understøtter nullable properties.
            modelBuilder.Entity<Booking>().OwnsOne(b => b.Discount);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.EmployeeSnapshot);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.CustomerSnapshot);
            modelBuilder.Entity<Booking>().OwnsOne(b => b.TreatmentSnapshot);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Invoice)
                .WithOne(i => i.Booking)
                .HasForeignKey<Invoice>(i => i.BookingId);

            //Selvom vores Booking relationer er nullable er det stadig nødvendigt at fortælle databasen at de må være null
            modelBuilder.Entity<Booking>().HasOne(b => b.Treatment).WithMany().OnDelete(DeleteBehavior.SetNull);

            //Vi fortæller eksplicit hvad foreign key på Booking tabllen skal hedde for kunden
            modelBuilder.Entity<PrivateCustomer>()
                .HasMany(c => c.Bookings)
                .WithOne(b => b.Customer)
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Employee)
                .HasForeignKey("EmployeeId")
                .OnDelete(DeleteBehavior.SetNull);

            //Pga. nogle andre konfigurationer vi har er det nødvendigt at fortælle EF at den skal bruge backing fieldet for propertien Bookings
            modelBuilder.Entity<PrivateCustomer>()
                .Navigation(c => c.Bookings)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            modelBuilder.Entity<Employee>()
                .Navigation(e => e.Bookings)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            //Vi ignorerer Total da den ikke har nogen setter men istedet har et backing field
            modelBuilder.Entity<Booking>().Ignore(b => b.TotalBase);
            //Vi mapper backing fieldet i stedet for propertien
            modelBuilder.Entity<Booking>().Property<decimal?>("_totalBase")
                .HasColumnName("TotalBase")
                .IsRequired(false);

            //Vi ignorerer Total da den ikke har nogen setter men istedet har et backing field
            modelBuilder.Entity<Booking>().Ignore(b => b.TotalWithDiscount);
            //Vi mapper backing fieldet i stedet for propertien
            modelBuilder.Entity<Booking>().Property<decimal?>("_totalWithDiscount")
                .HasColumnName("TotalWithDiscount")
                .IsRequired(false);

            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.Price);
            modelBuilder.Entity<Treatment>().ComplexProperty(t => t.DurationMinutes);

            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Name);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Email);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.PhoneNumber);
            modelBuilder.Entity<Employee>().ComplexProperty(e => e.Address);
            modelBuilder.Entity<Employee>()
                                            .HasMany(e => e.Treatments)
                                            .WithMany(t => t.Employees);

            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Name);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Email);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.PhoneNumber);
            modelBuilder.Entity<PrivateCustomer>().ComplexProperty(p => p.Address);

            modelBuilder.Entity<PrivateCustomer>().Ignore(p => p.Visits);

            modelBuilder.Entity<PrivateCustomer>()
                .Property<List<int>>("_birthdayDiscountUsedYears")
                .HasColumnName("BirthdayDiscountUsedYears")
                .IsRequired(false);

            modelBuilder.Entity<LoyaltyDiscount>().OwnsOne(l => l.DiscountPercent);
            modelBuilder.Entity<CampaignDiscount>().OwnsOne(c => c.DiscountPercent);
            modelBuilder.Entity<BirthdayDiscount>().OwnsOne(b => b.DiscountPercent);

            // Vi fortæller EF, at Id for Invoice ikke genereres af databasen
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Id).ValueGeneratedNever();
            });
        }
    }
}
