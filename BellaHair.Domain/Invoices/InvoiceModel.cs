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
        public int Id { get; private set; }
        public DateTime IssueDate { get; private set; }
        public CustomerSnapshot Customer { get; private set; }
        public List<TreatmentSnapshot> Treatments { get; private set; } = [];
        public BookingDiscount? Discount { get; private set; }
        public decimal Total { get; private set; }

        private InvoiceModel(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, decimal total, BookingDiscount? discount)
        {
            Id = id;
            IssueDate = issueDate;
            Customer = customer;
            Treatments.Add(treatment);
            Total = total;
            Discount = discount;
        }

        public static InvoiceModel Create(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, decimal total, BookingDiscount? discount)
        {
            return new InvoiceModel(id, issueDate, customer, treatment, total, discount);
        }
    }
}
