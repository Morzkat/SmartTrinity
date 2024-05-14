using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.App.Pumps.Core.Models
{
    public class Pump: BaseEntity
    {
        // TODO: Ask if is necessary a config for this.
        private bool _isAuthored = true;

        public int PumpNo { get; set; }
        public Grade Grade { get; set; }
        public string Status { get; set; }
        public double Volume { get; set; }
        public int PriceLevel { get; set; }
        public double SalePrice { get; set; }
        public IList<Hose> Hoses { get; set; }
        public double SaleProgress { get; set; }
        public bool IsAuthored { get { return _isAuthored; } }

        public void SetAthoredStatus(string status)
        {
            // HACK: Remove if| else if| else| statement and use approach more maintainable 
            if (status == PumpActions.AUTH.ToString())
                _isAuthored = true;
            else if (status == PumpActions.DEAUTH.ToString())
                _isAuthored = false;
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
        public int Pump { get; set; }
        public PumpActions Action { get; set; }
    }
}
