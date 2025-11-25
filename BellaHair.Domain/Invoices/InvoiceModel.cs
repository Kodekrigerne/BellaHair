using BellaHair.Domain.Bookings;

namespace BellaHair.Domain.Invoices
{
    public class InvoiceModel
    {
        public InvoiceModel(Guid invoiceNumber, DateTime issueDate, CustomerSnapshot customer, TreatmentSnapshot treatment, string comments)
        {
            InvoiceNumber = invoiceNumber;
            IssueDate = issueDate;
            Customer = customer;
            Comments = comments;
            Treatments.Add(treatment);
        }

        public Guid InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public CustomerSnapshot Customer { get; set; }
        public string Comments { get; set; }
        public List<TreatmentSnapshot> Treatments { get; set; } = [];
    }
}
