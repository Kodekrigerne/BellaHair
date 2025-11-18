using BellaHair.Domain;

namespace BellaHair.Infrastructure
{
    public class CurrentDateTimeProvider : ICurrentDateTimeProvider
    {
        DateTime ICurrentDateTimeProvider.GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
