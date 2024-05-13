namespace SmartTrinity.Core.Models
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public TrinitySettings TrinitySettings { get; set; }
    }

    public class TrinitySettings
    {
        public string TrinityConnectionString { get; set; }
        public int SalesLimitPerRequest { get; set; } = 100;
    }
}

