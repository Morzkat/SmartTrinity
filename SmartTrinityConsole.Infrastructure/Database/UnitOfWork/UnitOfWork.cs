using SmartTrinityApi.Core.Interfaces.Repository.Repositories;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Infrastructure.Database.Repositories;

namespace SmartTrinityConsole.Infrastructure.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IConfigValuesRepository ConfigValuesRepository { get; private set; }

        public IGenericConfigValuesRepository GenericConfigValuesRepository { get; private set; }

        public UnitOfWork() 
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            //HACK: Use DI for inject repositories.
            ConfigValuesRepository = new ConfigValuesRepository();
        }

        public int Commit()
        {
            return 0;
        }

        public void Dispose()
        {
        }
    }
}