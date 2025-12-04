using BellaHair.Domain.SharedValueObjects;

// Mikkel Dahlmann

namespace BellaHair.Domain.Products
{

    /// <summary>
    /// Represents a product with name, description and price.
    /// </summary>

    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Price Price { get; private set; }

#pragma warning disable CS8618
        private Product() { }
#pragma warning restore CS8618

        private Product(string name, string description, Price price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
        }

        public static Product Create(string name, string description, Price price) => new(name, description, price);

        public void Update(string name, string description, Price price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
