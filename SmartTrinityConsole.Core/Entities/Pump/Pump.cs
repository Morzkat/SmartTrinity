using System.Collections.Generic;
using SmartTrinityConsole.Core.Entities.Pump;

public class Pump
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
        if (status == "AUTH")
            _isAuthored = true;
        else if (status == "DEAUTH")
            _isAuthored = false;
    }
}

public class Hose 
{
    public int HoseId { get; set; }
    public double TotalizerMoney { get; set; }
    public double TotalizerVolume { get; set; }
    public IList<Grade> Grades { get; set; }
}

public class PumpServiceMode
{
    public int Id { get; set; }
    public int Pump { get; set; }
    public string ServiceMode { get; set; }
}

/**
    type 1 = MONEY
    type 2 = VOLUME
**/
public class PresetConfig
{
    public int Type { get; set; }
    public int PumpNo { get; set; }
    public double Amount { get; set; }
    public bool TankFull { get; set; }
    public IList<Grade> Grades { get; set; }
}

public class PumpAction
{
    public int Pump { get; set; }
    public string Action { get; set; }
}
