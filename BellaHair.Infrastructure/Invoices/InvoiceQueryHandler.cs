using BellaHair.Ports.Invoices;
using QuestPDF.Infrastructure;

namespace BellaHair.Infrastructure.Invoices
{
    public class InvoiceQueryHandler : IInvoiceQuery
    {
        public void CreateAndPrintInvoice()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var model = InvoiceDocumentDataSource.GetInvoiceDetails();
            var document = new InvoiceDocument(model);


            //document.ShowInCompanion(12500);
        }
    }
}