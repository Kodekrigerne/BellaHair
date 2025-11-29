// Mikkel Dahlmann

namespace BellaHair.Application.Invoices
{

    /// <summary>
    /// Represents configuration settings for business contact and identification information.
    /// Binds data from appsettings.json under the "BusinessInfo" section.
    /// </summary>

    public class BusinessInfoSettings
    {
        public const string SectionName = "BusinessInfo";

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CvrNumber { get; set; } = string.Empty;
    }
}
