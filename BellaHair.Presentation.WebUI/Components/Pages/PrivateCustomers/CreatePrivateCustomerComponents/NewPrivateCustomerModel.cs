// Mikkel Dahlmann

using System.ComponentModel.DataAnnotations;
using BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;
using BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents
{
    /// <summary>
    /// Represents the data required to create a new private customer, including personal, contact, and address
    /// information. Applies validation through attributes and trims the input where possible.
    /// </summary>
    
    public class NewPrivateCustomerModel
    {
        private string _firstName = string.Empty;
        private string? _middleName;
        private string _lastName = string.Empty;
        private string _streetName = string.Empty;
        private string _city = string.Empty;
        private string _streetNumber = string.Empty;
        private string _phoneNumber = string.Empty;
        private string _email = string.Empty;

        [Required(ErrorMessage = "Fornavn er påkrævet")]
        [LettersDashOnly]
        public string FirstName { get => _firstName; set => _firstName = value.Trim(); }

        [LettersDashOnly]
        public string? MiddleName { get => _middleName; set => _middleName = value?.Trim(); }

        [Required(ErrorMessage = "Efternavn er påkrævet")]
        [LettersDashOnly]
        public string LastName { get => _lastName; set => _lastName = value.Trim(); }

        [Required(ErrorMessage = "Vejnavn er påkrævet")]
        [StringLength(50, ErrorMessage = "Vejnavn kan ikke være længere end 50 bogstaver")]
        [LettersDashWhiteSpaceOnly]
        public string StreetName { get => _streetName; set => _streetName = value.Trim(); }

        [Required(ErrorMessage = "By er påkrævet")]
        [StringLength(50, ErrorMessage = "Navn på by må ikke være længere end 50 bogstaver")]
        [LettersDashWhiteSpaceOnly]
        public string City { get => _city; set => _city = value.Trim(); }

        [Required(ErrorMessage = "Husnummer er påkrævet")]
        [LettersNumbersOnly]
        public string StreetNumber { get => _streetNumber; set => _streetNumber = value.Trim(); }

        [Required(ErrorMessage = "Postnummer er påkrævet")]
        [ZipCode]
        public int? ZipCode { get; set; }

        [NumbersOnly]
        public int? Floor { get; set; }

        [Required(ErrorMessage = "Telefonnummber er påkrævet")]
        [DanishPhoneNumber]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value.Trim(); }

        [Required(ErrorMessage = "Email er påkrævet")]
        [Email]
        public string Email { get => _email; set => _email = value.Trim(); }

        [Required(ErrorMessage = "Fødselsdag er påkrævet")]
        [MustBeAdult]
        public DateTime? Birthday { get; set; }
    }
}