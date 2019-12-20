using Dapper.FluentMap.Mapping;
using Dapper.FluentMap.Dommel;
using SmartTrinityConsole.Core.Entities.Database.Configs;
using Dapper.FluentMap.Dommel.Mapping;

namespace SmartTrinityConsole.Infrastructure.Database.Config.FluentMap
{
    public class GenericConfigValuesMapper : DommelEntityMap<GenericConfigValues>
    {
        public GenericConfigValuesMapper()
        {
            // Map(e => e.Id).IsKey();
            Map(e => e.Id).ToColumn("id_d");
        }
    }
}