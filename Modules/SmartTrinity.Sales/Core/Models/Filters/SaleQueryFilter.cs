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
        public int? Limit { get; set; }
        public OrderByColumns? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; } = Filters.OrderDirection.Desc;
    }

    public enum OrderByColumns
    {
        SaleId,
        PumpId,
        HoseId,
        GradeId
    }

    public enum OrderDirection
    {
        Asc,
        Desc
    }
}
