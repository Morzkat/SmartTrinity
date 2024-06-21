namespace SmartTrinity.App.Pumps.Core.Dtos
{
    public record GradeDto
    {
        public int Id { get; set; }
        public int Red { get; set; }
        public int Blue { get; set; }
        public int Green { get; set; }
        public string RGB { get; set; }
        public string Description { get; set; }
        public IEnumerable<GradePriceDto> Prices { get; set; }
    }

    public record GradePriceDto
    {
        public double Price { get; set; }
        public int PriceLevel { get; set; }
    }
}
