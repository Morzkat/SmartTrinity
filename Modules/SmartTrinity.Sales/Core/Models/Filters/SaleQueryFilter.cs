using System;
namespace SmartTrinity.App.Sales.Core.Models.Filters
{
    public class SaleQueryFilter
    {
        public int? SaleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PumpId { get; set; }
        public int? HoseId { get; set; }
    }
}
