using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BellaHair.Application.Invoices
{

    // Mikkel Dahlmann

    /// <summary>
    /// Represents a component that displays customer contact information in a formatted layout.
    /// </summary>

    public class ContactComponent : IComponent
    {
        private string Address { get; }
        private string Name { get; }
        private string Email { get; }
        private string PhoneNumber { get; }
        private string Title { get; }
        private string CvrNumber { get; }


        public ContactComponent(string title, string address, string name, string email, string phoneNumber, string? cvrNumber = null)
        {
            Title = title;
            Address = address;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            CvrNumber = cvrNumber ?? "";
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Spacing(2);

                column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

                column.Item().Text(Name);
                column.Item().Text(Address);
                column.Item().Text($"Telefon: {PhoneNumber}");
                column.Item().Text($"Email: {Email}");
                if (CvrNumber != "") column.Item().Text($"CVR: {CvrNumber}");
            });
        }
    }
}
