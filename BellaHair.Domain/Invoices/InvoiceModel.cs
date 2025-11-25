using BellaHair.Domain.Treatments;

namespace BellaHair.Domain.Invoices
{
    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public Address BellaHairAddress { get; set; }
        public Address CustomerAddress { get; set; }

        public Treatment Treatment { get; set; }
        public string Comments { get; set; }
    }
}
