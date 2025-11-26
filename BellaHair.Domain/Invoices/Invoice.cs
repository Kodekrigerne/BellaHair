using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Invoices
{

    // Mikkel Dahlmann

    /// <summary>
    /// Represents an invoice associated with a booking, including its unique identifier and PDF content.
    /// </summary>
    public class Invoice
    {
        public int Id { get; private set; }
        public Byte[] InvoicePdf { get; private set; }
        public Booking Booking { get; private set; }
        public Guid BookingId { get; set; }

#pragma warning disable CS8618
        private Invoice() { }
#pragma warning restore CS8618

        private Invoice(int id, Booking booking, Byte[] invoicePdf)
        {
            Id = id;
            Booking = booking;
            InvoicePdf = invoicePdf;
        }

        public static Invoice Create(int id, Booking booking, Byte[] invoicePdf)
        {
            return new Invoice(id, booking, invoicePdf);
        }
    }

    public class InvoiceException(string message) : DomainException(message);
}
