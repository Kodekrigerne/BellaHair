namespace BellaHair.Domain.Invoices
{
    public class InvoiceModel
    {
        public int InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public InvoiceAddress SellerAddress { get; set; }
        public InvoiceAddress CustomerAddress { get; set; }

        public List<OrderItem> Items { get; set; }
        public string Comments { get; set; }
    }
}
