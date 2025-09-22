using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.App.Pumps.Core.Models
{
    public class Pump: BaseEntity
    {
        private bool _isAuthored = true;

        public int PumpNo { get; set; }
        public string Status { get; set; }
        public double Volume { get; set; }
        public int PriceLevel { get; set; }
        public double SalePrice { get; set; }
        public IList<Hose> Hoses { get; set; }
        public double SaleProgress { get; set; }
        public bool IsAuthored { get { return _isAuthored; } }

        public void SetAthoredStatus(PumpActions action)
        {
            _ = action switch
            {
                PumpActions.AUTH => _isAuthored = true,
                PumpActions.DEAUTH => _isAuthored = false,
                PumpActions.MONEY_PRESET => throw new NotImplementedException(),
                PumpActions.VOLUME_PRESET => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }
    }

    public class PumpWithAction: Pump { }

    public enum PumpActions
    {
        AUTH,
        DEAUTH,
        MONEY_PRESET,
        VOLUME_PRESET
    }

    public class PumpAction
    {
        public int PumpNo { get; set; }
        public PumpActions Action { get; set; }
    }
}
