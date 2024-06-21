namespace SmartTrinity.App.Pumps.Core.Dtos
{
    public record HoseDto
    {
        public int HoseId { get; set; }
        public double TotalizerMoney { get; set; }
        public double TotalizerVolume { get; set; }
        public IEnumerable<GradeDto> Grades { get; set; }
    }
}
