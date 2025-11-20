using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents
{
    public class NewPrivateCustomerModel
    {
        [Required(ErrorMessage = "Fornavn er påkrævet")]
        [LettersDashOnly]
        public string FirstName { get; set; } = string.Empty;

        [LettersDashOnly] public string? MiddleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Efternavn er påkrævet")]
        [LettersDashOnly]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vejnavn er påkrævet")]
        [StringLength(50, ErrorMessage = "Vejnavn kan ikke være længere end 50 bogstaver")]
        [LettersDashWhiteSpaceOnly]
        public string StreetName { get; set; } = string.Empty;

        [Required(ErrorMessage = "By er påkrævet")]
        [StringLength(50, ErrorMessage = "Navn på by må ikke være længere end 50 bogstaver")]
        [LettersDashWhiteSpaceOnly]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Husnummer er påkrævet")]
        [LettersNumbersOnly]
        public string StreetNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postnummer er påkrævet")]
        [ZipCode]
        public int ZipCode { get; set; } = 0;

        [NumbersOnly] public int? Floor { get; set; } = 0;

        [Required(ErrorMessage = "Telefonnummber er påkrævet")]
        [DanishPhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email er påkrævet")]
        [Email]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Alder er påkrævet")]
        [MustBeAdult]
        public DateTime Birthday { get; set; }

    }
}