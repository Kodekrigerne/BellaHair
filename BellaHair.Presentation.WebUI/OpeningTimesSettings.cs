namespace BellaHair.Presentation.WebUI
{
    public class OpeningTimesSettings
    {
        public const string SectionName = "OpeningTimes";

        public TimeOnly OpeningTime { get; set; } = default;
        public TimeOnly ClosingTime { get; set; } = default;
    }
}
