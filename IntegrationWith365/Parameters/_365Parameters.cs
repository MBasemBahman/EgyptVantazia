namespace IntegrationWith365.Parameters
{
    public class _365Parameters
    {
        public bool IsArabic { get; set; }

        public int LangId => IsArabic ? 27 : 1;

        public int UserCountryId { get; set; } = 131;

        public int Competitions { get; set; } = 552;

        public int AppTypeId { get; set; } = 5;

        public string TimezoneName { get; set; } = "Africa/Cairo";

    }
}
