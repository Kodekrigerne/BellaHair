using BellaHair.Domain.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;

// Mikkel Dahlmann

/// <summary>
/// Represents an invoice document that can be composed and rendered using the provided invoice model data.
/// </summary>

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }
    public Byte[] LogoContent { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "BellaHair.Application.Invoices.BellaHairLogo.png";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Could not find the embedded resource: {resourceName}");
        }

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        LogoContent = ms.ToArray();
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
                    .Text($"Faktura #{Model.Id}")
                    .FontSize(24).Bold().FontColor(Colors.Black);

                column.Item().Text(text =>
                {
                    text.Span("Fakturadato: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Betalt: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });
            });

            row.ConstantItem(150).Height(90).Image(LogoContent);
        });
    }

    public void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new CustomerComponent("Afsender:", "Nygade 1, 7100 Vejle", "BellaHair - CVR-nr: 12345678", "kontakt@bellahair.dk", "40284846"));
                row.ConstantItem(50);
                row.RelativeItem().Component(new CustomerComponent("Modtager:", Model.Customer.FullAddress, Model.Customer.FullName, Model.Customer.Email, Model.Customer.PhoneNumber));
            });

            column.Item().PaddingTop(30).Element(ComposeTable);

            var totalNoDiscountNoTax = Model.Total * 0.8m;
            var discountNoTax = Model.Discount.Amount * 0.8m;
            var totalWithDiscountNoTax = totalNoDiscountNoTax - discountNoTax;
            var tax = totalWithDiscountNoTax * 0.25m;
            var totalWithDiscountTax = totalWithDiscountNoTax * 1.25m;

            column.Item().PaddingTop(15).AlignRight().Text($"I alt ekskl. moms: kr {totalWithDiscountNoTax:N2}").FontSize(12);
            column.Item().AlignRight().Text($"Moms (25%): kr {tax:N2}").FontSize(12);
            column.Item().AlignRight().Text($"I alt inkl. moms: kr {totalWithDiscountTax:N2}").FontSize(15).SemiBold();

            //if (!string.IsNullOrWhiteSpace(Model.Comments))
            //    column.Item().PaddingTop(25).Element(ComposeComments);
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
                header.Cell().Element(CellStyle).AlignRight().Text("Enhedspris");
                header.Cell().Element(CellStyle).AlignRight().Text("Antal");
                header.Cell().Element(CellStyle).AlignRight().Text("Pris");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            foreach (var treatment in Model.Treatments)
            {
                table.Cell().Element(CellStyle).Text((Model.Treatments.IndexOf(treatment) + 1).ToString());
                table.Cell().Element(CellStyle).Text(treatment.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"kr {treatment.Price * 0.8m:N2}");
                table.Cell().Element(CellStyle).AlignRight().Text("1");
                table.Cell().Element(CellStyle).AlignRight().Text($"kr {treatment.Price * 0.8m * 1:N2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }

            if (Model.Discount.Amount > 0)
            {
                table.Cell().Element(CellStyle).Text("");
                table.Cell().Element(CellStyle).Text($"Rabat: {Model.Discount.Name}");
                table.Cell().Element(CellStyle).AlignRight().Text("");
                table.Cell().Element(CellStyle).AlignRight().Text("");
                table.Cell().Element(CellStyle).AlignRight().Text($"kr -{Model.Discount.Amount * 0.8m:N2}");

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

    //public void ComposeComments(IContainer container)
    //{
    //    container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
    //    {
    //        column.Spacing(5);
    //        column.Item().Text("Kommentarer").FontSize(14);
    //        column.Item().Text(Model.Comments);
    //    });
    //}
}