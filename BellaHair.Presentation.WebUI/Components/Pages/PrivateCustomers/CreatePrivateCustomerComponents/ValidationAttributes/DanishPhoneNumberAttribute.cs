using BellaHair.Domain.SharedValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static MudBlazor.Colors;

namespace BellaHair.Presentation.WebUI.Components.Pages.PrivateCustomers.CreatePrivateCustomerComponents.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists of 8 numbers.
/// </summary>

public class DanishPhoneNumberAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public DanishPhoneNumberAttribute() :
        base("Feltet kan kun indeholde et 8-cifret telefonnummer.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue) || stringValue.Length != 8 || !stringValue.All(Char.IsDigit))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }
}