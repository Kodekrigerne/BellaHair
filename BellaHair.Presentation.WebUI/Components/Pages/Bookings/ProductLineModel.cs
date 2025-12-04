namespace BellaHair.Presentation.WebUI.Components.Pages.Bookings
{
    public class ProductLineModel
    {
        public Guid ProductId { get; private init; }
        public string ProductName { get; private init; }
        public decimal Price { get; private init; }
        public int Quantity { get; private set; }

        public ProductLineModel(Guid productId, string productName, decimal price, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        public void SetQuantity(int newQuantity)
        {
            Quantity = newQuantity;
        }
    }
}
