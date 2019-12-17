using SmartTrinityApi.Core.Interfaces.Repository.Repositories;
using SmartTrinityConsole.Core.Entities.Configs;

namespace SmartTrinityConsole.Infrastructure.Database.Repositories
{
    public class ConfigValuesRepository : Repository<ConfigValues>, IConfigValuesRepository
    {
        protected override string tableName { get; set; }
        protected override string tableId {get;set;}

        public ConfigValuesRepository()
        {
            tableName = "CONFIG_VALUES";
            tableId = "id";
        }
    }
}