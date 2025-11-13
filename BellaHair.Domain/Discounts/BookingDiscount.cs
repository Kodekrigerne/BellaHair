namespace BellaHair.Domain.Discounts
{
    //Dennis
    /// <summary>
    /// Value object representing a discount to be applied to a specific booking.
    /// </summary>
    public record BookingDiscount
    {
        public string Name { get; private init; }
        public decimal Amount { get; private init; }
        public bool DiscountActive { get; private init; }

#pragma warning disable CS8618
        private BookingDiscount() { }
#pragma warning restore CS8618

        private BookingDiscount(string discountName, decimal discountAmount, bool discountActive)
        {
            Name = discountName;
            Amount = discountAmount;
            DiscountActive = discountActive;
        }

        public static BookingDiscount Active(string discountName, decimal discountAmount) => new(discountName, discountAmount, true);
        public static BookingDiscount Inactive(string discountName) => new(discountName, 0m, false);
    }
}
