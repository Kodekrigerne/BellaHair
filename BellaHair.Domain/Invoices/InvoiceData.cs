using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Invoices
{
    // Mikkel Dahlmann

    /// <summary>
    /// Represents an invoice, including customer information, issue date, treatments, and additional comments.
    /// </summary>

    public record InvoiceData
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public CustomerSnapshot Customer { get; set; }
        public TreatmentSnapshot Treatment { get; set; }
        public IEnumerable<ProductLineSnapshot> Products { get; set; }
        public BookingDiscount? Discount { get; set; }
        public decimal Total { get; set; }

        public InvoiceData(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, IEnumerable<ProductLineSnapshot> products, decimal total, BookingDiscount? discount)
        {
            Id = id;
            IssueDate = issueDate;
            Customer = customer;
            Treatment = treatment;
            Products = products;
            Total = total;
            Discount = discount;
        }
    }
}
