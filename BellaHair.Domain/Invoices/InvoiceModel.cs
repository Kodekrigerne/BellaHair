using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;

namespace BellaHair.Domain.Invoices
{
    // Mikkel Dahlmann

    /// <summary>
    /// Represents an invoice, including customer information, issue date, treatments, and additional comments.
    /// </summary>

    public class InvoiceModel
    {
        public InvoiceModel(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, BookingDiscount discount, decimal total)
        {
            Id = id;
            IssueDate = issueDate;
            Customer = customer;
            Treatments.Add(treatment);
            Discount = discount;
            Total = total;
        }

        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public CustomerSnapshot Customer { get; set; }
        public List<TreatmentSnapshot> Treatments { get; set; } = [];
        public BookingDiscount Discount { get; set; }
        public decimal Total { get; set; }
    }
}
