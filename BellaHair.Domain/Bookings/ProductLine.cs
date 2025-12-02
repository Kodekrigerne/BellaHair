using BellaHair.Domain.Products;

namespace BellaHair.Domain.Bookings
{
    public class ProductLine
    {
        public Quantity Quantity { get; private set; }
        public Product Product { get; private set; }

        private ProductLine(Quantity quantity, Product product)
        {
            Quantity = quantity;
            Product = product;
        }

        public static ProductLine Create(Quantity quantity, Product product) => new(quantity, product);
    }
}
