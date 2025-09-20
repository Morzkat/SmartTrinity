using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Pumps.Core.Dtos
{
    public record PresetDto
    {
        public int Type { get; set; }
        public int PumpNo { get; set; }
        public double Amount { get; set; }
        public bool TankFull { get; set; }
        public IEnumerable<FuelTypes>? Grades { get; set; }

        public string GradeToString (FuelTypes grade)
        {
            return grade switch
            {
                FuelTypes.RACING_GASOLINE => "GASOLINE RACING",
                FuelTypes.PREMIUN_GASOLINE => "PREMIUN_GASOLINE",
                FuelTypes.REGULAR_GASOLINE => "REGULAR_GASOLINE",
                _ => throw new NotImplementedException("El lado no tiene este producto disponible."),
            };
        }
    }

    public enum FuelTypes
    {
        RACING_GASOLINE,
        PREMIUN_GASOLINE,
        REGULAR_GASOLINE,
    }


}
