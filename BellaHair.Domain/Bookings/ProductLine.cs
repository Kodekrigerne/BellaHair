using BellaHair.Domain.Products;

namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Represents a product line on a booking with a quantity and a product.
    /// Should be owned and managed by Booking.
    /// </summary>
    public class ProductLine : EntityBase
    {
        public Quantity Quantity { get; private set; }
        public Product Product { get; private set; }

#pragma warning disable CS8618
        private ProductLine() { }
#pragma warning restore CS8618

        private ProductLine(Quantity quantity, Product product)
        {
            Id = Guid.NewGuid();
            Quantity = quantity;
            Product = product;
        }

        public static ProductLine Create(Quantity quantity, Product product) => new(quantity, product);
    }
}
