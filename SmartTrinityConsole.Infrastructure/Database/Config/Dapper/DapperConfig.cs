using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using SmartTrinityConsole.Infrastructure.Database.Config.FluentMap;

namespace SmartTrinityConsole.Infrastructure.Database.Config
{
    public static class DapperConfigurations
    {
        public static void ConfigureDapper()
        {
            // Dapper.CustomPropertyTypeMap
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

            // Mappers
            FluentMapper.Initialize(config => {
                config.AddMap(new GenericConfigValuesMapper());
                config.ForDommel();
            });
        }
    }
}