using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.App.Pumps.Core.Models
{
    [Table("config_values")]
    public class ConfigValues : BaseEntity
    {
        [Key]
        public override object Id { get; set; }
        public string Library { get; set; }
        public string GroupName { get; set; }
        public string DeviceId { get; set; }
        public string Parameter { get; set; }
        public string ParamValue { get; set; }
    }
}