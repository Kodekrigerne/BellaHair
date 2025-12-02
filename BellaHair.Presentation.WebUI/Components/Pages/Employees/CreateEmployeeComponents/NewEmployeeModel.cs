using BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Pages.Employees.CreateEmployeeComponents
{
    public class NewEmployeeModel
    {

        public Guid Id;
        private string _firstName = string.Empty;
        private string? _middleName = string.Empty;
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

        [Required(ErrorMessage = "Email er påkrævet")]
        [Email]
        public string Email { get => _email; set => _email = value.Trim(); }

        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        [DanishPhoneNumber]
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value.Trim(); }

        [Required(ErrorMessage = "Vejnavn er påkrævet")]
        [LettersNumbersOnly]
        public string StreetName { get => _streetName; set => _streetName = value.Trim(); }

        [Required(ErrorMessage = "Bynavn er påkrævet")]
        [LettersDashWhiteSpaceOnly]
        public string CityName { get => _city; set => _city = value.Trim(); }

        [Required(ErrorMessage = "Vejnummer er påkrævet")]
        [LettersNumbersOnly]
        public string StreetNumber { get => _streetNumber; set => _streetNumber = value.Trim(); }

        [Required(ErrorMessage = "Postnummer er påkrævet")]
        [ZipCode]
        public int ZipCode { get; set; }

        [NumbersOnly]
        public int? Floor { get; set; }

        public IEnumerable<Guid> TreatmentIds { get; set; } = [];
    }
}