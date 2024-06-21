namespace SmartTrinity.App.Pumps.Core.Dtos
{
    public record PumpDto
    {
        public int PumpNo { get; set; }
        public GradeDto Grade { get; set; }
        public string Status { get; set; }
        public double Volume { get; set; }
        public int PriceLevel { get; set; }
        public double SalePrice { get; set; }
        public IEnumerable<HoseDto> Hoses { get; set; }
        public double SaleProgress { get; set; }
    }
}
