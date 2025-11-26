using BellaHair.Domain.Invoices;

namespace BellaHair.Infrastructure.Invoices
{
    public class InvoiceChecker : IInvoiceChecker
    {
        private readonly BellaHairContext _db;

        public InvoiceChecker(BellaHairContext db)
        {
            _db = db;
        }

        async Task<bool> IInvoiceChecker.HasBeenPaid(Guid bookingId)
        {
            throw new NotImplementedException();
        }

        Task<bool> IInvoiceChecker.HasInvoice(Guid bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
