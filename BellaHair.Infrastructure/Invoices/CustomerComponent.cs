using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class CustomerComponent : IComponent
{
    private string Address { get; }
    private string Name { get; }
    private string Email { get; }
    private string PhoneNumber { get; }
    private string Title { get; }


    public CustomerComponent(string title, string address, string name, string email, string phoneNumber)
    {
        Title = title;
        Address = address;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
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
        });
    }
}