namespace BellaHair.Domain.Bookings
{
    //Dennis
    /// <summary>
    /// Represents a snapshot of a product line on a booking. These should be used rather than product lines when a booking has been paid.
    /// Contains all relevant information directly rather than containing a product snapshot
    /// This should be owned and managed by Booking.
    /// </summary>
    public record ProductLineSnapshot
    {
        public Guid Id { get; private init; }
        public Guid ProductId { get; private init; }
        public string Name { get; private init; }
        public string Description { get; private init; }
        public decimal Price { get; private init; }
        public int Quantity { get; private init; }

#pragma warning disable CS8618
        private ProductLineSnapshot() { }
#pragma warning restore CS8618

        private ProductLineSnapshot(ProductLine productLine)
        {
            Id = Guid.NewGuid();
            ProductId = productLine.Product.Id;
            Name = productLine.Product.Name;
            Description = productLine.Product.Description;
            Price = productLine.Product.Price.Value;
            Quantity = productLine.Quantity.Value;
        }

        public static ProductLineSnapshot FromProductLine(ProductLine productLine) => new(productLine);
    }
}
