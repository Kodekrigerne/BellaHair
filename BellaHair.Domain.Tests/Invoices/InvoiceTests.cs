using BellaHair.Domain.Bookings;
using BellaHair.Domain.Discounts;
using BellaHair.Domain.Invoices;
using FixtureBuilder;

namespace BellaHair.Domain.Tests.Invoices
{
    // Mikkel Dahlmann
    internal sealed class InvoiceTests
    {
        [Test]
        public void CreateInvoiceModel_Given_ValidInputs_Then_CreatesInvoiceModel()
        {
            // Arrange
            var id = 1;
            var issueDate = DateTime.Now;
            var customerSnapshot = Fixture.New<CustomerSnapshot>().Build();
            var treatmentSnapshot = Fixture.New<TreatmentSnapshot>().Build();
            var discount = Fixture.New<BookingDiscount>().Build();
            decimal total = 100;

            // Act
            var model = InvoiceModel.Create(id, issueDate, customerSnapshot, treatmentSnapshot, total, discount);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(id));
                Assert.That(model.IssueDate, Is.EqualTo(issueDate));
                Assert.That(model.Customer, Is.EqualTo(customerSnapshot));
                Assert.That(model.Treatments[0], Is.EqualTo(treatmentSnapshot));
                Assert.That(model.Discount, Is.EqualTo(discount));
                Assert.That(model.Total, Is.EqualTo(total));
            });
        }

        [Test]
        public void CreateInvoice_Given_ValidInputs_Then_CreatesInvoice()
        {
            // Arrange
            var id = 1;
            var booking = Fixture.New<Booking>().With(b => b.Id, Guid.NewGuid()).Build();
            var pdf = new Byte[] { 0x01, 0x02, 0x03 };

            // Act
            var invoice = Invoice.Create(id, booking, pdf);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(invoice.Id, Is.EqualTo(id));
                Assert.That(invoice.Booking, Is.EqualTo(booking));
                Assert.That(invoice.InvoicePdf[0], Is.EqualTo(pdf[0]));
                Assert.That(invoice.InvoicePdf[1], Is.EqualTo(pdf[1]));
                Assert.That(invoice.InvoicePdf[2], Is.EqualTo(pdf[2]));
                Assert.That(invoice.BookingId, Is.EqualTo(booking.Id));
            });
        }
    }
}
