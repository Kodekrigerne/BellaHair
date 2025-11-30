using System.ComponentModel.DataAnnotations;

namespace BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists of 8 numbers.
/// </summary>

public class DanishPhoneNumberAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public DanishPhoneNumberAttribute() :
        base("Skal være 8 cifre.")
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