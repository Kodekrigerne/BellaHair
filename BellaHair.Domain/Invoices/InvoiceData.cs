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
        public List<TreatmentSnapshot> Treatments { get; set; } = [];
        public BookingDiscount? Discount { get; set; }
        public decimal Total { get; set; }

        public InvoiceData(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, decimal total, BookingDiscount? discount)
        {
            Id = id;
            IssueDate = issueDate;
            Customer = customer;
            Treatments.Add(treatment);
            Total = total;
            Discount = discount;
        }
    }
}
