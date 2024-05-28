using SmartTrinity.App.Pumps.Core.Dtos;

namespace SmartTrinity.App.Pumps.Core.Models
{
    /**
      type 1 = MONEY
      type 2 = VOLUME
    **/
    public class Preset
    {
        public int Type { get; set; }
        public int PumpNo { get; set; }
        public double Amount { get; set; }
        public bool TankFull { get; set; } = false;
        public IEnumerable<GradeDto>? Grades { get; set; }
    }
}
