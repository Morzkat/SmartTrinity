using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SmartTrinity.Core.Database.Models;

namespace SmartTrinity.App.Pumps.Core.Models
{
    [Table("ssf_generic_config_values")]
    public class GenericConfigValues : BaseEntity
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