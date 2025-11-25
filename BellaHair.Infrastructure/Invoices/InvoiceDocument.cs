using BellaHair.Domain.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Mikkel Dahlmann

/// <summary>
/// Represents an invoice document that can be composed and rendered using the provided invoice model data.
/// </summary>

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    public void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .Text($"Faktura #{Model.InvoiceNumber}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Betalingsdato: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    public void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new CustomerComponent(Model.Customer.FullAddress, Model.Customer.FullName, Model.Customer.Email, Model.Customer.PhoneNumber));
                row.ConstantItem(50);
                row.RelativeItem().Component(new CustomerComponent(Model.Customer.FullAddress, Model.Customer.FullName, Model.Customer.Email, Model.Customer.PhoneNumber));
            });

            column.Item().Element(ComposeTable);

            var totalPrice = Model.Treatments.Sum(x => x.Price * 1);
            column.Item().AlignRight().Text($"Slut total: {totalPrice:C2}").FontSize(14);

            if (!string.IsNullOrWhiteSpace(Model.Comments))
                column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    public void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Service");
                header.Cell().Element(CellStyle).AlignRight().Text("Pris");
                header.Cell().Element(CellStyle).AlignRight().Text("Antal");
                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            foreach (var treatment in Model.Treatments)
            {
                table.Cell().Element(CellStyle).Text((Model.Treatments.IndexOf(treatment) + 1).ToString());
                table.Cell().Element(CellStyle).Text(treatment.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"{treatment.Price}");
                table.Cell().Element(CellStyle).AlignRight().Text("1");
                table.Cell().Element(CellStyle).AlignRight().Text($"{treatment.Price * 1:C2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

    public void ComposeComments(IContainer container)
    {
        container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Kommentarer").FontSize(14);
            column.Item().Text(Model.Comments);
        });
    }
}