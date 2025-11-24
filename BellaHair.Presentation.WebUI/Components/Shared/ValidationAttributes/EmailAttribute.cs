using BellaHair.Domain.SharedValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using static MudBlazor.Colors;

namespace BellaHair.Presentation.WebUI.Components.Shared.ValidationAttributes;

// Mikkel Dahlmann

/// <summary>
/// Specifies that a data field value is valid only if it consists of a valid email.
/// </summary>

public class EmailAttribute : ValidationAttribute
{
    // Overskriver standard fejlbeskeden.
    public EmailAttribute() :
        base("Feltet skal indeholde en valid email.")
    {
    }

    protected override ValidationResult? IsValid(object? input, ValidationContext validationContext)
    {
        // Kører validering på input.
        if (input is string stringValue)
        {
            if (!ValidEmailRegex.IsMatch(stringValue))
            {
                return new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            }
        }

        return ValidationResult.Success;
    }

    private static readonly Regex ValidEmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
}

