using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Treatments
{
    // Mikkel Klitgaard

    /// <summary>
    /// Represents the data required to create a new treatment, including name, price, and duration.
    /// </summary>

    public class NewTreatmentModel
    {
        [Required(ErrorMessage = "Navn er påkrævet")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Navn må kun indeholde bogstaver og tal")]
        public string Name { get; set; } = "";

        [Range(1, 100000, ErrorMessage = "Prisen skal være mellem 1 og 100.000 kr.")]
        public decimal Price { get; set; }


        [Range(10, 300, ErrorMessage = "Varighed skal være mellem 10 og 300 min.")]
        public int DurationMinutes { get; set; }
    }
}
