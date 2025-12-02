using BellaHair.Domain.Products;

namespace BellaHair.Domain.Bookings
{
    public class ProductLine
    {
        public Quantity Quantity { get; private set; }
        public Product Product { get; private set; }

#pragma warning disable CS8618
        private ProductLine() { }
#pragma warning restore CS8618

        private ProductLine(Quantity quantity, Product product)
        {
            Quantity = quantity;
            Product = product;
        }

        public static ProductLine Create(Quantity quantity, Product product) => new(quantity, product);
    }
}
