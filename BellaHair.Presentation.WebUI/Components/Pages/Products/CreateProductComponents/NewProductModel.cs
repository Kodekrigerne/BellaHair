using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Products.CreateProductComponents
{
    public class NewProductModel
    {
        public Guid Id { get; set; }

        private string _name = string.Empty;

        private string _description = string.Empty;

        private decimal _price;

        [Required(ErrorMessage = "Navn er påkrævet")]
        [StringLength(100, ErrorMessage = "Navn kan ikke være længere end 100 tegn")]
        public string Name { get => _name; set => _name = FormatName(value.Trim()); }

        [Required(ErrorMessage = "Beskrivelse er påkrævet")]
        [StringLength(500, ErrorMessage = "Beskrivelse kan ikke være længere end 500 tegn")]
        public string Description { get => _description; set => _description = value.Trim(); }

        [Required(ErrorMessage = "Pris er påkrævet")]
        [Range(0.01, 100000.00, ErrorMessage = "Prisen skal være mellem 0,01 og 100.000,00 kr.")]
        public decimal Price { get => _price; set => _price = value; }


        private string FormatName(string? input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string firstChar = input[0].ToString().ToUpperInvariant();
            string restOfString = input.Substring(1).ToLowerInvariant();

            return firstChar + restOfString;
        }
    }
}
