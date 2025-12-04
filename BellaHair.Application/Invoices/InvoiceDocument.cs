using BellaHair.Application.Invoices;
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
    public InvoiceData Data { get; }
    public Byte[] LogoContent { get; }
    public BusinessInfoSettings BusinessInfoSettings { get; }
    public InvoiceDocument(InvoiceData data, BusinessInfoSettings businessInfoSettings)
    {
        Data = data;
        BusinessInfoSettings = businessInfoSettings;

        // Logo er gemt som embedded resource, dvs. skrevet ind i .dll-filen.
        // For at tilgå logo fra .dll skal vi bruge Assembly klassen, og
        // "udlæse" logoet som en stream.
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
                    .Text($"Faktura #{Data.Id}")
                    .FontSize(24).Bold().FontColor(Colors.Black);

                column.Item().Text(text =>
                {
                    text.Span("Fakturadato: ").SemiBold();
                    text.Span($"{Data.IssueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Betalt: ").SemiBold();
                    text.Span($"{Data.IssueDate:d}");
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
                row.RelativeItem().Component(new ContactComponent("Afsender:", BusinessInfoSettings.Address, BusinessInfoSettings.Name, BusinessInfoSettings.Email, BusinessInfoSettings.PhoneNumber, BusinessInfoSettings.CvrNumber));
                row.ConstantItem(50);
                row.RelativeItem().Component(new ContactComponent("Modtager:", Data.Customer.FullAddress, Data.Customer.FullName, Data.Customer.Email, Data.Customer.PhoneNumber));
            });

            column.Item().PaddingTop(30).Element(ComposeTable);

            if (Data.Discount != null)
            {
                var totalNoDiscountNoTax = Data.Total * 0.8m;
                var discountNoTax = Data.Discount.Amount * 0.8m;
                var totalWithDiscountNoTax = totalNoDiscountNoTax - discountNoTax;
                var tax = totalWithDiscountNoTax * 0.25m;
                var totalWithDiscountTax = totalWithDiscountNoTax * 1.25m;

                column.Item().PaddingTop(15).AlignRight().Text($"I alt ekskl. moms: kr {totalWithDiscountNoTax:N2}").FontSize(12);
                column.Item().AlignRight().Text($"Moms (25%): kr {tax:N2}").FontSize(12);
                column.Item().AlignRight().Text($"I alt inkl. moms: kr {totalWithDiscountTax:N2}").FontSize(15).SemiBold();
            }
            else
            {
                var totalNoDiscountNoTax = Data.Total * 0.8m;
                var tax = totalNoDiscountNoTax * 0.25m;
                var totalWithTax = totalNoDiscountNoTax * 1.25m;

                column.Item().PaddingTop(15).AlignRight().Text($"I alt ekskl. moms: kr {totalNoDiscountNoTax:N2}").FontSize(12);
                column.Item().AlignRight().Text($"Moms (25%): kr {tax:N2}").FontSize(12);
                column.Item().AlignRight().Text($"I alt inkl. moms: kr {totalWithTax:N2}").FontSize(15).SemiBold();
            }
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

            table.Cell().Element(CellStyle).Text(("1"));
            table.Cell().Element(CellStyle).Text(Data.Treatment.Name);
            table.Cell().Element(CellStyle).AlignRight().Text($"kr {Data.Treatment.Price * 0.8m:N2}");
            table.Cell().Element(CellStyle).AlignRight().Text("1");
            table.Cell().Element(CellStyle).AlignRight().Text($"kr {Data.Treatment.Price * 0.8m * 1:N2}");

            static IContainer CellStyle(IContainer container)
            {
                return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
            }

            foreach (var product in Data.Products)
            {
                table.Cell().Element(CellStyle2).Text((Data.Products.IndexOf(product) + 1).ToString());
                table.Cell().Element(CellStyle2).Text(Data.Treatment.Name);
                table.Cell().Element(CellStyle2).AlignRight().Text($"kr {Data.Treatment.Price * 0.8m:N2}");
                table.Cell().Element(CellStyle2).AlignRight().Text("1");
                table.Cell().Element(CellStyle2).AlignRight().Text($"kr {Data.Treatment.Price * 0.8m * 1:N2}");

                static IContainer CellStyle2(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }

            if (Data.Discount != null)
            {
                table.Cell().Element(CellStyle1).Text("");
                table.Cell().Element(CellStyle1).Text($"Rabat: {Data.Discount.Name}");
                table.Cell().Element(CellStyle1).AlignRight().Text("");
                table.Cell().Element(CellStyle1).AlignRight().Text("");
                table.Cell().Element(CellStyle1).AlignRight().Text($"kr -{Data.Discount.Amount * 0.8m:N2}");

                static IContainer CellStyle1(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}