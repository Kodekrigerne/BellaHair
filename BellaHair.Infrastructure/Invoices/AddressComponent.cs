using BellaHair.Domain;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class AddressComponent : IComponent
{
    private string Title { get; }
    private Address Address { get; }

    public AddressComponent(string title, Address address)
    {
        Title = title;
        Address = address;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

            column.Item().Text(Address.StreetName);
            column.Item().Text(Address.City);
            column.Item().Text(Address.Email);
            column.Item().Text(Address.Phone);
        });
    }
}