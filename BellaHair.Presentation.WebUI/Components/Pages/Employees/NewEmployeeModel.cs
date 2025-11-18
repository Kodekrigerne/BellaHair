using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees
{
    public class NewEmployeeModel
    {

        [Required(ErrorMessage = "Fornavn er påkrævet")]
        [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Fornavn må ikke indeholde specialtegn.")]
        public string FirstName { get; set; } = "";

        [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Mellemnavn må ikke indeholde specialtegn.")]
        public string MiddleName { get; set; } = "";

        [Required(ErrorMessage = "Efternavn er påkrævet")]
        [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Efternavn må ikke indeholde specialtegn.")]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage = "Email er påkrævet")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email er ikke udfyldt korrekt.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        [Phone(ErrorMessage = "Telefonnummer er ikke udfyldt korrekt.")]
        [Range(8, 8, ErrorMessage = "Telefonnummer skal være 8 cifre.")]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessage = "Vejnavn er påkrævet")]
        [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Vejnavn må ikke indeholde specialtegn.")]
        public string StreetName { get; set; } = "";

        [Required(ErrorMessage = "Bynavn er påkrævet")]
        [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Bynavn må ikke indeholde specialtegn.")]
        public string CityName { get; set; } = "";

        [Required(ErrorMessage = "Vejnummer er påkrævet")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Vejnummer skal være et tal.")]
        public string StreetNumber { get; set; } = "";

        [Required(ErrorMessage = "Postnummer er påkrævet")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Postnummer skal bestå af tal.")]
        [Range(4, 4, ErrorMessage = "Vejnavn skal være på 4 cifre.")]
        public int ZipCode { get; set; }

        public string? Floor { get; set; } = "";

        public List<Guid> TreatmentIds { get; set; } = [];
    }
}