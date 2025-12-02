namespace BellaHair.Domain.Bookings
{
    public record ProductLineSnapshot
    {
        public Guid ProductLineId { get; private init; }
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
            ProductLineId = productLine.Id;
            ProductId = productLine.Product.Id;
            Name = productLine.Product.Name;
            Description = productLine.Product.Description;
            Price = productLine.Product.Price.Value;
            Quantity = productLine.Quantity.Value;
        }

        public static ProductLineSnapshot FromProductLine(ProductLine productLine) => new(productLine);
    }
}
