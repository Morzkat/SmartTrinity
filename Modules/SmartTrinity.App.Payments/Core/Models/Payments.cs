
using SmartTrinity.Core.Database.Models;

public class Payment : BaseEntity
{
    public string Date { get; set; }
    public string method { get; set; }
    public string SystemSale { get; set; }
    public string UserSale { get; set; }
    public int SaleId { get; set; }
    public int Shift { get; set; }
    public string VeriphoneType { get; set; }
    public int PaymentId { get; set; }
    public string Client { get; set; }
    public string Rnc { get; set; }
    public string OtherData { get; set; }
    public string OtherType { get; set; }
    public string Card { get; set; }
    public string Plate { get; set; }
    public decimal Amount { get; set; }
    public decimal Itbis { get; set; }
    public decimal Money { get; set; }
    public string TallyId { get; set; }
    public string FuelStationAttendant { get; set; }
}