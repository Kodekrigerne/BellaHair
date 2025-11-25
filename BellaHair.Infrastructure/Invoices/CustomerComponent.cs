using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

public class CustomerComponent : IComponent
{
    private string Address { get; }
    private string Name { get; }
    private string Email { get; }
    private string PhoneNumber { get; }


    public CustomerComponent(string address, string name, string email, string phoneNumber)
    {
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

            column.Item().Text(Name);
            column.Item().Text(Address);
            column.Item().Text($"Telefon: {PhoneNumber}");
            column.Item().Text($"Email: {Email}");
        });
    }
}