
using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.App.FuelStation.Core.Models
{
    public class Station : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Rnc { get; set; }
        public string Telephone { get; set; }
    }
}
