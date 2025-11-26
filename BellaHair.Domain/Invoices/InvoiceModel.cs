using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Invoices
{
    // Mikkel Dahlmann

    /// <summary>
    /// Represents an invoice, including customer information, issue date, treatments, and additional comments.
    /// </summary>

    public class InvoiceModel
    {
        public InvoiceModel(int id, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment)
        {
            Id = id;
            IssueDate = issueDate;
            Customer = customer;
            Treatments.Add(treatment);
        }

        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public CustomerSnapshot Customer { get; set; }
        public List<TreatmentSnapshot> Treatments { get; set; } = [];
    }
}
